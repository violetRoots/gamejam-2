using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BaseProjectile
{
    [SerializeField] private float speed = 1.0f;

    [SerializeField] private Rigidbody2D bulletRigidbody;

    private Vector2 _direction;


#if UNITY_EDITOR
    private void OnValidate()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }
#endif

    public void Init(Vector2 direction, int damage)
    {
        _direction = direction;
        SetInitTime();
        SetDamage(damage);

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Vector2.SignedAngle(Vector2.right, direction));
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        bulletRigidbody.velocity = _direction * speed * Time.fixedDeltaTime;
    }


}
