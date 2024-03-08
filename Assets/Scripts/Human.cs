using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] protected float baseSpeed;
    [SerializeField] protected float lerpSpeed = 5.0f;

    [ReadOnly(true)]
    [SerializeField] protected CircleCollider2D _circleCollider;

    [ReadOnly(true)]
    [SerializeField] protected Rigidbody2D _humanRigidbody;
    protected Vector3 _distinationPoint;

    protected Vector3 _velocity;
    protected Vector3 _previousVelocity;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _humanRigidbody = GetComponent<Rigidbody2D>();
    }
#endif

    public void SetDestinationPosition(Vector3 destinationPoint)
    {
        _distinationPoint = destinationPoint;
    }

    protected virtual void Update() { }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent(out Human human)) return;

        _humanRigidbody.velocity = Vector3.zero;
    }

    protected virtual void Move()
    {
        if (Vector2.Distance(transform.position, _distinationPoint) < 0.1f)
        {
            _humanRigidbody.velocity = Vector3.zero;
            return;
        }

        _velocity = (_distinationPoint - transform.position).normalized * GetSpeed();

        _humanRigidbody.velocity = Vector2.Lerp(_humanRigidbody.velocity, _velocity, lerpSpeed * Time.fixedDeltaTime);

        _previousVelocity = _velocity;
    }

    protected virtual void Rotate()
    {

    }

    protected virtual float GetSpeed()
    {
        return baseSpeed;
    }
}
