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
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float distantionToRun = 10.0f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 1.0f;
    [SerializeField] private int framesToFlip = 100;
    [SerializeField] private Transform rotationLogicContainer;
    [SerializeField] private Transform rotationVisualContainer;
    [SerializeField] private Transform staticVisualContainer;

    [Header("Shooter")]
    [SerializeField] private HumanShooterController shooterController;

    private Vector3 _targetVelocity;
    private Vector3 _currentVelocity;
    private Vector3 _velocityDamp;

    private Vector3 _currentRotationPartScale;
    private Vector3 _currentStaticPartScale;
    private bool _isFlip;
    private int _canFlipCount;

    protected override void OnEnable()
    {
        base.OnEnable();

        _currentRotationPartScale = rotationVisualContainer.localScale;
        _currentStaticPartScale = staticVisualContainer.localScale;
    }
    protected override void Move()
    {
        //_targetVelocity = (_destinationPosition - transform.position).normalized * GetSpeed();
        //_currentVelocity = Vector3.SmoothDamp(_currentVelocity, _targetVelocity, ref _velocityDamp, Time.fixedDeltaTime * dampMultiplier);
        //_humanRigidbody.velocity = _currentVelocity;

        transform.position = _destinationPosition;
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
        shooterController.CheckShoot();
    }

    protected override float GetSpeed()
    {
        var distance = Vector2.Distance(transform.position, _destinationPosition);
        var rotationDirection = _inputManager.RotateDirection;
        var moveDirection = _inputManager.MoveDirection;
        if (rotationDirection.magnitude > 0)
        {
            var addiction = Vector2.Dot(moveDirection, rotationDirection) * moveDirection.magnitude * 0.5f;
            var runValue = distance / distantionToRun;
            if (runValue >= rotationDirection.magnitude)
            {
                return runSpeed * (runValue + addiction) * Mathf.Clamp01(distance / distantionToWalk);
            }
            else
            {
                return walkSpeed * (_inputManager.RotateDirection.magnitude + addiction) * Mathf.Clamp01(distance / distantionToWalk);
            }
        }
        else if (moveDirection.magnitude > 0)
        {
            return walkSpeed * _inputManager.MoveDirection.magnitude * Mathf.Clamp01(distance / distantionToWalk);
        }
        else
        {
            return 0;
        }
    }

    protected override void SetStartHealth()
    {
        float newStartHealth = startHealth;
        if (_skillManager.IsSkillApplied(out RotatableHumanHpUpSkill rotatableHumanHpUpSkillConfig))
            newStartHealth += startHealth * rotatableHumanHpUpSkillConfig.hpFactorMultiplier / 100.0f;

        Health = (int) newStartHealth;
    }
}
