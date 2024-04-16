using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletShooterController : HumanShooterController
{
    [Header("Bullets")]
    [MinMaxSlider(0.0f, 90.0f)]
    [SerializeField] private Vector2 randomBulletAngleOffset = Vector2.up;

    [Space(10)]
    [SerializeField] private Transform bulletContainer;
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

        var bullet = Instantiate(bulletPrefab, bulletContainer.position, Quaternion.identity);
        var angleOffset = Random.Range(randomBulletAngleOffset.x, randomBulletAngleOffset.y);
        var direction = Quaternion.Euler(0.0f, 0.0f, angleOffset) * bulletContainer.right;

        var bulletDamage = CalculateDamage();

        bullet.Init(direction, bulletDamage);
    }
}
