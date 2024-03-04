using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBullet : MonoBehaviour
{
    private Vector3 _direction;
    private float _speed;
    private float _initTime;

    public void Init(Vector3 direction, float speed)
    {
        _direction = direction;
        _speed = speed;
        _initTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - _initTime > 20.0f)
            Destroy(gameObject);

        transform.position += _direction * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

        OnCollide();

        if (collision.TryGetComponent(out DemoHuman human))
        {
            if (!DemoCrowdController.Instance.TryKill(human, true))
                human.Die();
        }
        else if(collision.TryGetComponent(out DemoDynamicEnemy enemy))
        {
            enemy.Die();
        }
    }

    private void OnCollide()
    {
        Destroy(gameObject);
    }
}
