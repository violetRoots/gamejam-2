using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashEnemy : Enemy
{
    [Header("Splash")]
    [SerializeField] private float splashRadius = 10.0f;

    public override void Damage()
    {
        Splash();

        base.Die();
    }

    private void Splash()
    {
        var hits = Physics2D.CircleCastAll(transform.position, splashRadius, Vector2.zero);
        foreach (var hit in hits)
        {
            if (!hit.collider.TryGetComponent(out IDamagable damagable)) continue;

            damagable.Die();
        }
    }
}
