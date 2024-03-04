using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float lerpSpeed = 5.0f;

    [ReadOnly(true)]
    [SerializeField] protected CircleCollider2D _circleCollider;

    [ReadOnly(true)]
    [SerializeField] protected Rigidbody2D _humanRigidbody;
    protected Vector3 _distinationPoint;


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

    private void FixedUpdate()
    {
        MoveToDestinationPoint();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent(out Human human)) return;

        _humanRigidbody.velocity = Vector3.zero;
    }

    private void MoveToDestinationPoint()
    {
        if (Vector2.Distance(transform.position, _distinationPoint) < 0.1f)
        {
            _humanRigidbody.velocity = Vector3.zero;
            return;
        }

        var velocity = (_distinationPoint - transform.position).normalized * GetSpeed();
        _humanRigidbody.velocity = Vector2.Lerp(_humanRigidbody.velocity, velocity, lerpSpeed * Time.fixedDeltaTime);
    }

    protected virtual float GetSpeed()
    {
        return speed;
    }
}
