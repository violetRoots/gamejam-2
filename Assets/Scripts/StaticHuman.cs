using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticHuman : Human
{
    [SerializeField] private float repulsionCastRadius = 20.0f;
    [SerializeField] private float lerpSpeedChangeMultiplier = 1.0f;

    protected override void Move()
    {
        var hits = Physics2D.CircleCastAll(transform.position, repulsionCastRadius, Vector2.zero);
        var rotatableHits = hits.Where(hit => hit.collider.TryGetComponent(out RotatableHuman rotatableHuman)).ToArray();

        if (rotatableHits.Length > 0)
        {
            _velocity = rotatableHits[0].normal * GetSpeed();
        }
        else
        {
            if (Vector2.Distance(transform.position, _distinationPoint) < 0.1f)
            {
                _humanRigidbody.velocity = Vector3.zero;
                return;
            }

            _velocity = (_distinationPoint - transform.position).normalized * GetSpeed();
        }

        _velocity = Vector3.Lerp(_previousVelocity, _velocity, Time.fixedDeltaTime * lerpSpeedChangeMultiplier);

        _humanRigidbody.velocity = Vector2.Lerp(_humanRigidbody.velocity, _velocity, lerpSpeed * Time.fixedDeltaTime);

        _previousVelocity = _velocity;
    }
}
