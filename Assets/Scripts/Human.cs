using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] protected float speed;

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
        var hit = Physics2D.Raycast(transform.position, _distinationPoint - transform.position, _circleCollider.radius + speed * 1000.0f * Time.fixedDeltaTime);
        if (hit.collider.TryGetComponent(out Human human) && human != this)
        {
            Debug.Log(hit.collider.name);
            _humanRigidbody.velocity = Vector3.zero;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _distinationPoint, speed * Time.fixedDeltaTime);
    }
}
