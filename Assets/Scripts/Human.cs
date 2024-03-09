using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Human : MonoBehaviour
{
    [SerializeField] protected float baseSpeed;
    [SerializeField] protected float lerpSpeed = 5.0f;
    [SerializeField] protected float dampMultiplier = 1.0f;

    [ReadOnly(true)]
    [SerializeField] protected CircleCollider2D _circleCollider;

    [ReadOnly(true)]
    [SerializeField] protected Rigidbody2D _humanRigidbody;

    protected CrowdController _crowdController;

    protected Vector3 _distinationPoint;
    protected Vector3 _targetVelocity;
    protected Vector3 _currentVelocity;
    protected Vector3 _velocityDamp;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _humanRigidbody = GetComponent<Rigidbody2D>();
    }
#endif

    protected virtual void Awake()
    {
        _crowdController = CrowdController.Instance;
    }

    protected virtual void Update() { }

    private void FixedUpdate()
    {
        if (!IsInCrowd()) return;

        Move();
        Rotate();
    }

    public void SetDestinationPosition(Vector3 destinationPoint)
    {
        _distinationPoint = destinationPoint;
    }

    public void Die()
    {
        if (IsInCrowd())
        {
            _crowdController.RemoveHuman(this);
        }

        Destroy(gameObject);
    }

    protected virtual void Move()
    {
        var stoppingDistance = 0.5f;

        if (Vector2.Distance(transform.position, _distinationPoint) < stoppingDistance)
        {
            _targetVelocity = Vector3.zero;
        }
        else
        {
            _targetVelocity = (_distinationPoint - transform.position).normalized * GetSpeed();
        }

        _currentVelocity = Vector3.SmoothDamp(_currentVelocity, _targetVelocity, ref _velocityDamp, Time.fixedDeltaTime * dampMultiplier);

        _humanRigidbody.velocity = _currentVelocity;
    }

    protected virtual void Rotate()
    {

    }

    protected virtual float GetSpeed()
    {
        return baseSpeed;
    }

    private bool IsInCrowd()
    {
        return _crowdController.HasHuman(this);
    }
}
