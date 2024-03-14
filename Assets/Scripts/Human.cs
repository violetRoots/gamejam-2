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

    protected Vector3 _destinationPosition;
    protected float _destinationAngleOffset;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _humanRigidbody = GetComponent<Rigidbody2D>();
    }
#endif

    protected abstract void Move();

    protected abstract void Rotate();

    protected abstract void SkillAction();

    protected abstract float GetSpeed();

    protected virtual bool CanMove()
    {
        return IsInCrowd();
    }

    protected virtual bool CanRotate()
    {
        return IsInCrowd();
    }

    protected virtual bool CanUseSkill()
    {
        return IsInCrowd();
    }

    protected virtual void Awake()
    {
        _inputManager = InputManager.Instance;
        _crowdController = CrowdController.Instance;

        _destinationPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if(CanMove()) 
            Move();

        if(CanRotate()) 
            Rotate();

        if(CanUseSkill())
            SkillAction();
    }

    protected bool IsInCrowd()
    {
        return _crowdController.HasHuman(this);
    }

    public virtual bool CanCollect()
    {
        return true;
    }

    public void SetDestinationPosition(Vector3 destinationPosition)
    {
        _destinationPosition = destinationPosition;
    }

    public void SetAngleOffset(float angleOffset)
    {
        _destinationAngleOffset = angleOffset;
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

    public virtual bool CanDamage()
    {
        return true;
    }
}
