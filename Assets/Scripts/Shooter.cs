using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Human
{
    [Space]
    [SerializeField] private float bulletsSpeed = 10.0f;
    
    [Space]
    [SerializeField] private Transform bulletOrigin;

    [Space]
    [SerializeField] private Bullet bullet;

    private void OnDisable()
    {
        if (InputManager.Instance == null) return;

        InputManager.Instance.OnLeftMouseButtonDown -= Shoot;
    }

    public override void AddInCrowd()
    {
        base.AddInCrowd();

        InputManager.Instance.OnLeftMouseButtonDown += Shoot;
    }

    private void Shoot()
    {
        Debug.Log("SHOOT");

        var newBullet = Instantiate(bullet, bulletOrigin.position, Quaternion.identity);
        newBullet.Init(transform.right, bulletsSpeed);
    }
}
