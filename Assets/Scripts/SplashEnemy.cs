using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashEnemy : Enemy
{
    [Header("Splash")]
    [SerializeField] private int splashDamage = 500;
    [SerializeField] private float splashRadius = 10.0f;

    private bool _canSplash = true;

    public override void GetDamage(int damagePoints)
    {
        if (_canSplash)
        {
            _canSplash = false;
            Splash();
        }

        base.GetDamage(damagePoints);
    }

    private void Splash()
    {
        var hits = Physics2D.CircleCastAll(transform.position, splashRadius, Vector2.zero);
        foreach (var hit in hits)
        {
            if (!hit.collider.TryGetComponent(out IDamagable damagable) || damagable == (IDamagable) this) continue;

            if(damagable.CanGetDamage())
                damagable.GetDamage(splashDamage);
        }
    }
}
