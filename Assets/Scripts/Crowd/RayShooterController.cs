using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooterController : HumanShooterController
{
    [SerializeField] private Transform rayContainer;
    [SerializeField] private BulletRay rayPrefab;

    public override void CheckShoot()
    {
        base.CheckShoot();

        if (!CanShoot()) return;

        Shoot();
    }

    protected override void Shoot()
    {
        base.Shoot();

        var bullet = Instantiate(rayPrefab, rayContainer.position, Quaternion.identity);
        var direction = rayContainer.right;

        var bulletDamage = CalculateDamage();

        bullet.Init(direction, bulletDamage);
    }
}
