using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticHuman : Human, IBulletDamagable
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4.0f;
    [SerializeField] private float walkDistantion = 0.1f;
    [SerializeField] protected float moveDampMultiplier = 1.0f;
    [SerializeField] private float repulsionCastRadius = 20.0f;
    [SerializeField] private float repulsionSpeedMultiplier = 2.0f;

    private bool _isSaved;

    private Vector3 _targetVelocity;
    private Vector3 _currentVelocity;
    private Vector3 _dampVelocity;

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

    protected override void SkillAction() { }

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
}
