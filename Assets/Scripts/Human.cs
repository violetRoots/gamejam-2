using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Human : MonoBehaviour
{
    [SerializeField] protected float speed;

    protected Rigidbody2D _rigidbody2D;
    protected CircleCollider2D _circleCollider;
    protected Transform _distinationPoint;
    protected bool _inCrowd;

    protected virtual void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (!_inCrowd) return;

        MoveToDestinationPoint();
    }

    public virtual void AddInCrowd()
    {
        _inCrowd = true;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public void SetDestinationPoint(Transform destinationPoint)
    {
        _distinationPoint = destinationPoint;
    }

    private void MoveToDestinationPoint()
    {
        var hit = Physics2D.Raycast(transform.position, _distinationPoint.position - transform.position, _circleCollider.radius + speed * Time.fixedDeltaTime);
        if (hit.collider.TryGetComponent(out Human human) && human != this) return;

        transform.position = Vector3.MoveTowards(transform.position, _distinationPoint.position, speed * Time.fixedDeltaTime);
    }
}
