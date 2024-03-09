using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float dampMultiplier = 1.0f;
    [SerializeField] private float castRadius = 5.0f;

    [ReadOnly(true)]
    [SerializeField] private Rigidbody2D enemyRigidbody;

    private Vector3 _currentVelocity;
    private Vector3 _targetVelocity;
    private Vector3 _dampVelocity;

#if UNITY_EDITOR
    private void OnValidate()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }
#endif

    private void FixedUpdate()
    {
        var hits = Physics2D.CircleCastAll(transform.position, castRadius, Vector2.zero);
        var humanhits = hits.Where(hit => hit.collider.TryGetComponent(out Human human)).ToArray();

        if (humanhits.Length > 0)
        {
            _targetVelocity = (humanhits[0].collider.transform.position - transform.position).normalized * speed * Time.fixedDeltaTime;
        }
        else
        {
            _targetVelocity = Vector3.zero;
        }

        _currentVelocity = Vector3.SmoothDamp(_currentVelocity, _targetVelocity, ref _dampVelocity, dampMultiplier * Time.fixedDeltaTime);
        enemyRigidbody.velocity = _currentVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent(out Human human)) return;

        human.Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
