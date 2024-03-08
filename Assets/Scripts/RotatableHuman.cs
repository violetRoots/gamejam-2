using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RotatableHuman : Human
{
    [SerializeField] private float speedOnRotate;
    [SerializeField] private float rechargeTime;
    [SerializeField] private float bulletCastRadius = 0.5f;
    [SerializeField] private float bulletCastDistance = 2.0f;

    [SerializeField] private Transform bulletContainer;
    [SerializeField] private Bullet bulletPrefab;
    
    private InputManager _inputManager;
    private float _lastShootTime;
    private void Start()
    {
        _inputManager = InputManager.Instance;
    }

    protected override void Update()
    {
        CheckShoot();
    }

    protected override float GetSpeed()
    {
        if(_inputManager.RotateDirection.magnitude > 0)
        {
            return speedOnRotate * _inputManager.RotateDirection.magnitude;
        }
        else if (_inputManager.MoveDirection.magnitude > 0)
        {
            return baseSpeed * _inputManager.MoveDirection.magnitude;
        }
        else
        {
            return 0;
        }
    }

    protected override void Rotate()
    {
        if (_velocity.magnitude <= 0) return;

        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, _inputManager.RotateDirection));
    }

    private void CheckShoot()
    {
        var hits = Physics2D.CircleCastAll(transform.position, bulletCastRadius, transform.rotation * Vector3.right, bulletCastDistance);
        var hasEnemy = hits.Any(hit => hit.collider.TryGetComponent(out Enemy enemy));

        if(!hasEnemy) return;

        if (!CanShoot()) return;

        Shoot();
    }

    private bool CanShoot()
    {
        return Time.time - _lastShootTime >= rechargeTime;
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, bulletContainer.position, Quaternion.identity);
        bullet.Init(transform.rotation);

        _lastShootTime = Time.time; 
    }
}
