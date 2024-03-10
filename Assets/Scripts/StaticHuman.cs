using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticHuman : Human, IBulletDamagable
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4.0f;
    [SerializeField] private float walkDistantion = 0.1f;
    [SerializeField] protected float lerpSpeedChangeMultiplier = 1.0f;
    [SerializeField] private float repulsionCastRadius = 20.0f;

    private Vector3 _targetVelocity;
    private Vector3 _currentVelocity;

    protected override void Move()
    {
        var hits = Physics2D.CircleCastAll(transform.position, repulsionCastRadius, Vector2.zero);
        var rotatableHits = hits.Where(hit => hit.collider.TryGetComponent(out RotatableHuman rotatableHuman)).ToArray();

        if (rotatableHits.Length > 0)
        {
            _targetVelocity = (rotatableHits[0].normal + new Vector2(-rotatableHits[0].normal.y, rotatableHits[0].normal.x)).normalized * GetSpeed();
        }
        else
        {
            if (Vector2.Distance(transform.position, DestinationPosition) < walkDistantion)
            {
                _humanRigidbody.velocity = Vector3.zero;
                return;
            }

            _targetVelocity = (DestinationPosition - transform.position).normalized * GetSpeed();
        }

        _targetVelocity = Vector3.Lerp(_currentVelocity, _targetVelocity, Time.fixedDeltaTime * lerpSpeedChangeMultiplier);

        _humanRigidbody.velocity = _targetVelocity;// Vector2.Lerp(_humanRigidbody.velocity, _velocity, lerpSpeed * Time.fixedDeltaTime);

        _currentVelocity = _targetVelocity;
    }

    protected override void Rotate() { }

    protected override void ActionOnFixedUpdate() { }

    protected override float GetSpeed()
    {
        return walkSpeed;
    }
}
