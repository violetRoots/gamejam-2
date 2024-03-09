using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public abstract class Human : MonoBehaviour
{
    [ReadOnly(true)]
    [SerializeField] protected CircleCollider2D _circleCollider;

    [ReadOnly(true)]
    [SerializeField] protected Rigidbody2D _humanRigidbody;

    protected InputManager _inputManager;
    protected CrowdController _crowdController;

    protected Vector3 _distinationPoint;

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
}
