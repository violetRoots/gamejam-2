using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicEnemy : Enemy
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float aggresionDistance = 10.0f;
    [SerializeField] private GameObject bloodEfect;

    private Transform crowdTransform;

    private void Start()
    {
        crowdTransform = CrowdController.Instance.transform;
    }

    private void Update()
    {
        var distance = Vector2.Distance(crowdTransform.position, transform.position);
        if (distance >= aggresionDistance || CrowdController.Instance.IsWin) return;

        transform.position = Vector3.MoveTowards(transform.position, crowdTransform.position, speed * Time.deltaTime);
    }

    public void Die()
    {
        AudioManager.Instance.PlayEnemySound();

        Instantiate(bloodEfect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
