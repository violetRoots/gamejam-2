using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticHuman : Human, IDamagable
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4.0f;
    [SerializeField] private float walkDistantion = 0.1f;
    [SerializeField] protected float moveDampMultiplier = 1.0f;
    [SerializeField] private float repulsionCastRadius = 20.0f;
    [SerializeField] private float repulsionSpeedMultiplier = 2.0f;

    [Header("Experience")]
    [SerializeField] private int experiencePointsToReborn = 50;
    [SerializeField] private float experienceEffectDuration = 0.2f;

    [Space]
    [SerializeField] private RotatableHuman rotatableHumanPrefab;
    [SerializeField] private SpriteRenderer experienceEffectSpriteRenderer;
    [SerializeField] private Transform experienceEffectMaskTransform;

    [Header("Mine")]
    [SerializeField] private float mineSpawnTimeout = 0.5f;
    [SerializeField] private float mineSpawnDistance = 2.0f;
    [SerializeField] private float mineBombTimeout = 4.5f;

    [Space]
    [SerializeField] private Mine minePrefab;

    [Header("Thorns")]
    [SerializeField] private ThornsController thornsController;

    private bool _isSaved;

    private Vector3 _targetVelocity;
    private Vector3 _currentVelocity;
    private Vector3 _dampVelocity;

    private int _currentExperiencePoints;
    private Tweener _experienceEffectTweener;
    private Color _startColor;
    private Color _clearColor;

    private float _lastMineSpawnTime;
    private Vector3 _lastMineSpawnPosition;
    private float _randomMineSpawnTimeOffset;

    protected override void OnEnable()
    {
        base.OnEnable();

        _startColor = experienceEffectSpriteRenderer.color;
        _clearColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        _randomMineSpawnTimeOffset = Random.value;
    }

    protected override void Move()
    {
        var hits = Physics2D.CircleCastAll(transform.position, repulsionCastRadius, Vector2.zero);
        var rotatableHits = hits.Where(hit => hit.collider.TryGetComponent(out RotatableHuman rotatableHuman)).ToArray();

        var repulsionVelocity = Vector3.zero;
        var normal = Vector3.zero;
        var destinationDir = (_destinationPosition - transform.position).normalized;
        var hitDistance = 0.0f;
        if (rotatableHits.Length > 0)
        {
            var distance = Vector2.Distance(rotatableHits[0].transform.position, transform.position);
            hitDistance = rotatableHits[0].distance;
            normal = rotatableHits[0].normal;
            var newDir = ((Vector2) normal * 0.5f + new Vector2(-rotatableHits[0].normal.y, rotatableHits[0].normal.x)).normalized;
            var dir = Vector2.Dot(destinationDir, newDir) > 0 ? newDir : -newDir;
            repulsionVelocity = dir * repulsionSpeedMultiplier;
        }

        _targetVelocity = (destinationDir + repulsionVelocity + normal).normalized * GetSpeed();

        _currentVelocity = Vector3.SmoothDamp(_currentVelocity, _targetVelocity, ref _dampVelocity, moveDampMultiplier * Time.fixedDeltaTime);
        _humanRigidbody.velocity = _currentVelocity;
    }

    protected override void Rotate() { }

    protected override void SkillAction()
    {
        if(_skillManager.IsSkillApplied<MineSkill>())
        {
            if (CanSpawnMine())
                SpawnMine();
        }

        var activateThorns = _skillManager.IsSkillApplied<ThornsSkill>();
        thornsController.gameObject.SetActive(activateThorns);

        //if(CanHeal())
        //    Heal();
    }

    protected override float GetSpeed()
    {
        var distance = Vector2.Distance(transform.position, _destinationPosition);
        return walkSpeed * Mathf.Clamp01(distance / walkDistantion);
    }
    public override bool CanCollect()
    {
        return !_isSaved;
    }

    protected override bool CanMove()
    {
        return IsInCrowd() || _isSaved;
    }

    public override bool CanGetDamage()
    {
        return !_isSaved;
    }

    public void Save()
    {
        _isSaved = true;
    }

    public override void AddExperiencePoints(int points)
    {
        _currentExperiencePoints += points;
        experienceEffectMaskTransform.localScale = new Vector3(1.0f, 1.0f - (float)_currentExperiencePoints / experiencePointsToReborn, 1.0f);

        if(_currentExperiencePoints >= CalculateExperiencePointsToReborn())
        {
            Reborn();

            Die();
        }
        else
        {
            ExperienceEffect();
        }
    }

    private void ExperienceEffect()
    {
        _experienceEffectTweener?.Kill();
        _experienceEffectTweener = null;

        experienceEffectSpriteRenderer.color = _startColor;
        _experienceEffectTweener = experienceEffectSpriteRenderer.DOColor(_clearColor, experienceEffectDuration);
    }

    private void Reborn()
    {
        var rotatableHuman = Instantiate(rotatableHumanPrefab, transform.position, Quaternion.identity);
        _crowdController.AddHuman(rotatableHuman);
    }

    private int CalculateExperiencePointsToReborn()
    {
        float newExpPoints = experiencePointsToReborn;
        if (_skillManager.IsSkillApplied(out RebornUpSkill rebornSkillConfig))
            newExpPoints += experiencePointsToReborn * rebornSkillConfig.experienceFactorMultiplier / 100.0f;

        return (int) newExpPoints;
    }

    private bool CanSpawnMine()
    {
        return (Time.time - _lastMineSpawnTime >= mineSpawnTimeout + mineSpawnTimeout * _randomMineSpawnTimeOffset
               && Vector2.Distance(_lastMineSpawnPosition, transform.position) >= mineSpawnDistance)
               || Time.time - _lastMineSpawnTime >= mineSpawnTimeout + mineBombTimeout;
    }

    private void SpawnMine()
    {
        var mine = Instantiate(minePrefab, transform.position, Quaternion.identity);
        mine.Init(mineBombTimeout);

        _lastMineSpawnTime = Time.time;
        _lastMineSpawnPosition = transform.position;
    }
}
