using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour, IBulletDamagable
{
    [Header("General")]
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float dampMultiplier = 1.0f;
    [SerializeField] private float moveRadius = 50.0f;

    [Space(10)]
    [ReadOnly(true)]
    [SerializeField] private Rigidbody2D enemyRigidbody;

    private CrowdController _crowdController;

    private Vector3 _currentVelocity;
    private Vector3 _targetVelocity;
    private Vector3 _dampVelocity;

#if UNITY_EDITOR
    private void OnValidate()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }
#endif

    private void Start()
    {
        _crowdController = CrowdController.Instance;
    }

    private void FixedUpdate()
    {
        var humans = _crowdController.GetHumanInfos().Select(info => info.human).ToArray();
        var nearestHuman = GetNearestHuman(humans);

        if (nearestHuman != null && Vector2.Distance(transform.position, nearestHuman.transform.position) < moveRadius)
        {
            _targetVelocity = (nearestHuman.transform.position - transform.position).normalized * speed * Time.fixedDeltaTime;
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

    private Human GetNearestHuman(IEnumerable<Human> humans)
    {
        return humans.OrderBy(human => Vector2.Distance(transform.position, human.transform.position)).FirstOrDefault();
    }

    public virtual void Damage()
    {
        Die();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
