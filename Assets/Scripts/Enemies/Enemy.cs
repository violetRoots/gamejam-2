using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : Creature, IProjectileDamagable
{
    [Header("General")]
    [SerializeField] private int damage = 100;
    [SerializeField] private float damageOffset = 5.0f;

    [Header("Movement")]
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float dampMultiplier = 1.0f;

    [Space(10)]
    [System.ComponentModel.ReadOnly(true)]
    [SerializeField] private Rigidbody2D enemyRigidbody;

    [Header("Damage Effect")]
    [SerializeField] private float damageEffectDuration = 0.2f;

    [Space(10)]
    [SerializeField] private SpriteRenderer damageEffectSpriteRenderer;

    [Header("Experience")]
    [SerializeField]
    [MinMaxSlider(1, 20)] private Vector2Int experiencePointsBounds = Vector2Int.one;
    [SerializeField] private float experienceSpawnRadius = 3.0f;

    [Space(10)]
    [SerializeField] private Experience experiencePrefab;

    protected CrowdController _crowdController;

    private Vector3 _currentVelocity;
    private Vector3 _targetVelocity;
    private Vector3 _dampVelocity;

    private Color _clearColor;
    private Tweener _damageEffectTweener;

#if UNITY_EDITOR
    private void OnValidate()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }
#endif

    protected override void OnEnable()
    {
        base.OnEnable();

        _clearColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    private void Start()
    {
        _crowdController = CrowdController.Instance;
    }

    private void FixedUpdate()
    {
        var humans = _crowdController.GetHumanInfos().Select(info => info.human).ToArray();
        var nearestHuman = GetNearestHuman(humans);

        if (nearestHuman != null)
        {
            _targetVelocity = (nearestHuman.transform.position - transform.position).normalized * speed * Time.fixedDeltaTime;
        }
        else
        {
            _targetVelocity = Vector3.zero;
        }

        _currentVelocity = Vector3.SmoothDamp(_currentVelocity, _targetVelocity, ref _dampVelocity, dampMultiplier * Time.fixedDeltaTime);
        enemyRigidbody.velocity = _currentVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent(out Human human)) return;

        if (human.CanGetDamage())
        {
            human.GetDamage(damage);
            transform.position += (Vector3) collision.contacts[0].normal * damageOffset;
        }
    }

    private Human GetNearestHuman(IEnumerable<Human> humans)
    {
        return humans.Where(human => human != null).OrderBy(human =>  Vector2.Distance(transform.position, human.transform.position)).FirstOrDefault();
    }

    public override void GetDamage(int damagePoints)
    {
        Health -= damagePoints;

        if (Health > 0)
        {
            DamageEffect();
        }
        else
        {
            Die();
        }
    }

    public override void DieInternal()
    {
        _damageEffectTweener?.Kill();
        _damageEffectTweener = null;

        base.DieInternal();
    }

    public override bool CanGetDamage()
    {
        return true;
    }

    private void DamageEffect()
    {
        _damageEffectTweener?.Kill();
        _damageEffectTweener = null;

        damageEffectSpriteRenderer.color = Color.white;
        _damageEffectTweener = damageEffectSpriteRenderer.DOColor(_clearColor, damageEffectDuration);
    }
}
