using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletShooterController : HumanShooterController
{
    [SerializeField] private float randomBulletAngleOffset = 30;

    [Space(10)]
    [SerializeField] private Transform shootOriginPoint;
    [SerializeField] private Bullet bulletPrefab;

    public override void CheckShoot()
    {
        base.CheckShoot();

        if (!CanShoot()) return;

        Shoot();
    }

    protected override void Shoot()
    {
        base.Shoot();

        var bullet = Instantiate(bulletPrefab, shootOriginPoint.position, Quaternion.identity);
        var angleOffset = Random.Range(-randomBulletAngleOffset, randomBulletAngleOffset);
        var direction = Quaternion.Euler(0.0f, 0.0f, angleOffset) * shootOriginPoint.right;

        var bulletDamage = CalculateDamage();

        bullet.Init(direction, bulletDamage);
    }
}
