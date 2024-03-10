using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public abstract class Human : MonoBehaviour, IDamagable
{
    [ReadOnly(true)]
    [SerializeField] protected CircleCollider2D _circleCollider;

    [ReadOnly(true)]
    [SerializeField] protected Rigidbody2D _humanRigidbody;

    protected InputManager _inputManager;
    protected CrowdController _crowdController;

    protected BasePositionPoint _destinationPoint;
    protected Vector3 DestinationPosition => _destinationPoint == null ? Vector3.zero : _destinationPoint.transform.position;
    protected float DestinationAngleOffset => _destinationPoint == null ? 0 : _destinationPoint.AngleOffset;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _humanRigidbody = GetComponent<Rigidbody2D>();
    }
#endif

    protected abstract void Move();

    protected abstract void Rotate();

    protected abstract void ActionOnFixedUpdate();

    protected abstract float GetSpeed();

    protected virtual void Awake()
    {
        _inputManager = InputManager.Instance;
        _crowdController = CrowdController.Instance;
    }

    private void FixedUpdate()
    {
        if (!IsInCrowd()) return;

        Move();
        Rotate();
        ActionOnFixedUpdate();
    }

    private bool IsInCrowd()
    {
        return _crowdController.HasHuman(this);
    }

    public void SetDestinationPosition(BasePositionPoint destinationPoint)
    {
        _destinationPoint = destinationPoint;
    }

    public virtual void Die()
    {
        if (IsInCrowd())
        {
            _crowdController.RemoveHuman(this);
        }

        Destroy(gameObject);
    }

    public virtual void Damage()
    {
        Die();
    }
}
