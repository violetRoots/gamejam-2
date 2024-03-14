using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticHuman : Human, IBulletDamagable
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4.0f;
    [SerializeField] private float repulsionSpeed = 8.0f;
    [SerializeField] private float walkDistantion = 0.1f;
    [SerializeField] protected float moveDampMultiplier = 1.0f;
    [SerializeField] private float repulsionCastRadius = 20.0f;

    private bool _isSaved;

    private Vector3 _targetVelocity;
    private Vector3 _currentVelocity;
    private Vector3 _dampVelocity;

    protected override void Move()
    {
        var hits = Physics2D.CircleCastAll(transform.position, repulsionCastRadius, Vector2.zero);
        var rotatableHits = hits.Where(hit => hit.collider.TryGetComponent(out RotatableHuman rotatableHuman)).ToArray();

        if (rotatableHits.Length > 0)
        {
            var destinationDir = (_destinationPosition - transform.position).normalized;
            var newDir = (rotatableHits[0].normal * 0.5f + new Vector2(-rotatableHits[0].normal.y, rotatableHits[0].normal.x)).normalized;
            var dir = Vector2.Dot(destinationDir, newDir) > 0 ? newDir : -newDir;
            _targetVelocity = dir * repulsionSpeed;
        }
        else
        {
            if (Vector2.Distance(transform.position, _destinationPosition) >= walkDistantion)
            {
                _targetVelocity = (_destinationPosition - transform.position).normalized * GetSpeed();
            }
            else
            {
                _targetVelocity = Vector3.zero;
            }
        }

        _currentVelocity = Vector3.SmoothDamp(_currentVelocity, _targetVelocity, ref _dampVelocity, moveDampMultiplier * Time.fixedDeltaTime);

        //_targetVelocity = Vector3.Lerp(_currentVelocity, _targetVelocity, Time.fixedDeltaTime * lerpSpeedChangeMultiplier);

        _humanRigidbody.velocity = _currentVelocity;// Vector2.Lerp(_humanRigidbody.velocity, _velocity, lerpSpeed * Time.fixedDeltaTime);
    }

    protected override void Rotate() { }

    protected override void SkillAction() { }

    protected override float GetSpeed()
    {
        return walkSpeed;
    }
    public override bool CanCollect()
    {
        return !_isSaved;
    }

    protected override bool CanMove()
    {
        return IsInCrowd() || _isSaved;
    }

    public override bool CanDamage()
    {
        return !_isSaved;
    }

    public void Save()
    {
        _isSaved = true;
    }
}
