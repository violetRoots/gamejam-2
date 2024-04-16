using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] private float destroyTimeout = 5.0f;
    [SerializeField] private float collisionDestroyTimeout = 0.0f;

    private bool _destroyCollisionProcessEnabled;
    private float _initTime;
    private int _damage;

    protected void SetInitTime()
    {
        _initTime = Time.time;
    }

    protected void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IProjectileDamagable damagable))
        {
            if (damagable.CanGetDamage())
                damagable.GetDamage(_damage);

            if (!_destroyCollisionProcessEnabled)
            {
                _destroyCollisionProcessEnabled = true;

                StartCoroutine(CollisionDestroyProcess());
            }
        }
    }

    private void Update()
    {
        if (CheckDestroy())
        {
            Destroy(gameObject);
            return;
        }
    }

    private bool CheckDestroy()
    {
        return Time.time - _initTime > destroyTimeout;
    }

    private IEnumerator CollisionDestroyProcess()
    {
        yield return new WaitForSeconds(collisionDestroyTimeout);

        Destroy(gameObject);
    }
}
