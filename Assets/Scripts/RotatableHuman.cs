using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RotatableHuman : Human
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float distantionToWalk = 0.5f;
    [SerializeField] private float dampMultiplier = 1.0f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 1.0f;

    [Header("Bullets")]
    [SerializeField] private float rechargeTime;
    [SerializeField] private float bulletCastRadius = 0.5f;
    [SerializeField] private float bulletCastDistance = 2.0f;

    [Space(10)]
    [SerializeField] private Transform bulletContainer;
    [SerializeField] private Bullet bulletPrefab;

    private Vector3 _targetVelocity;
    private Vector3 _currentVelocity;
    private Vector3 _velocityDamp;

    private float _lastShootTime;

    protected override void Move()
    {
        _targetVelocity = (_destinationPosition - transform.position).normalized * GetSpeed();
        _currentVelocity = Vector3.SmoothDamp(_currentVelocity, _targetVelocity, ref _velocityDamp, Time.fixedDeltaTime * dampMultiplier);
        _humanRigidbody.velocity = _currentVelocity;
    }

    protected override void Rotate()
    {
        if (_inputManager.RotateDirection.magnitude <= 0) return;

        var angle = Vector2.SignedAngle(Vector2.right, _inputManager.RotateDirection) + _destinationAngleOffset;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.fixedDeltaTime);
    }

    protected override void SkillAction()
    {
        CheckShoot();
    }

    protected override float GetSpeed()
    {
        if (_inputManager.RotateDirection.magnitude > 0)
        {
            if (Vector2.Distance(transform.position, _destinationPosition) > distantionToWalk)
                return walkSpeed * _inputManager.RotateDirection.magnitude;
            else
                return 0;
        }
        else if (_inputManager.MoveDirection.magnitude > 0)
        {
            if (Vector2.Distance(transform.position, _destinationPosition) > distantionToWalk)
                return walkSpeed * _inputManager.MoveDirection.magnitude;
            else
                return 0;
        }
        else
        {
            return 0;
        }
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
