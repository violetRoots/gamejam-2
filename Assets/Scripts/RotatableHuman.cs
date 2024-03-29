using NaughtyAttributes;
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
    [SerializeField] private int framesToFlip = 100;
    [SerializeField] private Transform rotationLogicContainer;
    [SerializeField] private Transform rotationVisualContainer;
    [SerializeField] private Transform staticVisualContainer;

    [Header("Bullets")]
    [SerializeField] private int damage = 100;
    [SerializeField] private float rechargeTime;
    [SerializeField] private float bulletCastRadius = 0.5f;
    [SerializeField] private float bulletCastDistance = 2.0f;
    [MinMaxSlider(0.0f, 90.0f)]
    [SerializeField] private Vector2 randomBulletAngleOffset = Vector2.up;

    [Space(10)]
    [SerializeField] private Transform bulletContainer;
    [SerializeField] private Bullet bulletPrefab;

    private Vector3 _targetVelocity;
    private Vector3 _currentVelocity;
    private Vector3 _velocityDamp;

    private Vector3 _currentRotationPartScale;
    private Vector3 _currentStaticPartScale;
    private bool _isFlip;
    private int _canFlipCount;

    private float _lastShootTime;

    protected override void Awake()
    {
        base.Awake();

        _currentRotationPartScale = rotationVisualContainer.localScale;
        _currentStaticPartScale = staticVisualContainer.localScale;
    }
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
        rotationVisualContainer.rotation = Quaternion.Lerp(rotationVisualContainer.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.fixedDeltaTime);

        if (Mathf.Abs(angle) >= 90.0f && !_isFlip || Mathf.Abs(angle) < 90.0f && _isFlip)
            _canFlipCount++;
        else
            _canFlipCount = 0;

        if(_canFlipCount >= framesToFlip)
        {
            _isFlip = !_isFlip;
            _currentRotationPartScale.y *= -1;
            _currentStaticPartScale.x *= -1;
        }

        rotationVisualContainer.localScale = _currentRotationPartScale;
        staticVisualContainer.localScale = _currentStaticPartScale;
    }

    protected override void SkillAction()
    {
        CheckShoot();
    }

    protected override float GetSpeed()
    {
        var distance = Vector2.Distance(transform.position, _destinationPosition);
        if (_inputManager.RotateDirection.magnitude > 0) 
        {
            return walkSpeed * _inputManager.RotateDirection.magnitude * Mathf.Clamp01(distance / distantionToWalk);
        }
        else if (_inputManager.MoveDirection.magnitude > 0)
        {
            return walkSpeed * _inputManager.MoveDirection.magnitude * Mathf.Clamp01(distance / distantionToWalk);
        }
        else
        {
            return 0;
        }
    }

    private void CheckShoot()
    {
        var hits = Physics2D.CircleCastAll(transform.position, bulletCastRadius, bulletContainer.right, bulletCastDistance);
        var enemyTransform = hits.Where(hit => hit.collider.TryGetComponent(out Enemy enemy)).Select(hit => hit.transform).FirstOrDefault();

        if(enemyTransform == null) return;

        if (!CanShoot()) return;

        Shoot(enemyTransform.position);
    }

    private bool CanShoot()
    {
        return Time.time - _lastShootTime >= rechargeTime;
    }

    private void Shoot(Vector3 enemyPos)
    {
        var bullet = Instantiate(bulletPrefab, bulletContainer.position, Quaternion.identity);
        var angleOffset = Random.Range(randomBulletAngleOffset.x, randomBulletAngleOffset.y);
        var direction = Quaternion.Euler(0.0f, 0.0f, angleOffset) * bulletContainer.right;
        bullet.Init(direction, damage);

        _lastShootTime = Time.time; 
    }

    //protected override bool CanMove()
    //{
    //    return _inputManager.RotateDirection.magnitude > 0;
    //}
}
