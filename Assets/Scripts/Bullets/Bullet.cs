using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float destroyTimeout = 5.0f;
    [SerializeField] private float speed = 1.0f;

    [SerializeField] private Rigidbody2D bulletRigidbody;

    private Vector2 _direction;
    private float _initTime;
    private int _damage;

#if UNITY_EDITOR
    private void OnValidate()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }
#endif

    public void Init(Vector2 direction, int damage)
    {
        _initTime = Time.time;
        _direction = direction;
        _damage = damage;

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Vector2.SignedAngle(Vector2.right, direction));
    }

    private void Update()
    {
        if (CheckDestroy())
        {
            Destroy(gameObject);
            return;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IBulletDamagable damagable))
        {
            if (damagable.CanGetDamage())
                damagable.GetDamage(_damage);

            Destroy(gameObject);
        }
    }

    private void Move()
    {
        bulletRigidbody.velocity = _direction * speed * Time.fixedDeltaTime;
    }

    private bool CheckDestroy()
    {
        return Time.time - _initTime > destroyTimeout;
    }
}
