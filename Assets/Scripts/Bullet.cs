using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float destroyTimeout = 5.0f;
    [SerializeField] private float speed = 1.0f;

    [SerializeField] private Rigidbody2D bulletRigidbody;

    private Quaternion _direction;
    private float _initTime;

#if UNITY_EDITOR
    private void OnValidate()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }
#endif

    public void Init(Quaternion direction)
    {
        _initTime = Time.time;
        _direction = direction;
    }

    private void Update()
    {
        CheckDestroy();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Enemy enemy))
        {
            enemy.Die();
            Destroy(gameObject);
        }
        else if(collision.TryGetComponent(out Human human))
        {
            human.Die();
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        bulletRigidbody.velocity = _direction * Vector3.right * speed * Time.fixedDeltaTime;
    }

    private void CheckDestroy()
    {
        if (Time.time - _initTime <= destroyTimeout) return;

        Destroy(gameObject);
    }
}
