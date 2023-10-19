using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float aggresionDistance = 10.0f;

    private Transform crowdTransform;

    private void Start()
    {
        crowdTransform = CrowdController.Instance.transform;
    }

    private void Update()
    {
        var distance = Vector2.Distance(crowdTransform.position, transform.position);
        if (distance >= aggresionDistance) return;

        transform.position = Vector3.MoveTowards(transform.position, crowdTransform.position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent(out Human human)) return;

        CrowdController.Instance.Kill(human);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
