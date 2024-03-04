using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoDynamicEnemy : DemoEnemy
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float aggresionDistance = 10.0f;
    [SerializeField] private GameObject bloodEfect;

    private Transform crowdTransform;

    private void Start()
    {
        crowdTransform = DemoCrowdController.Instance.transform;
    }

    private void Update()
    {
        var distance = Vector2.Distance(crowdTransform.position, transform.position);
        if (distance >= aggresionDistance || DemoCrowdController.Instance.IsWin) return;

        transform.position = Vector3.MoveTowards(transform.position, crowdTransform.position, speed * Time.deltaTime);
    }

    public void Die()
    {
        DemoAudioManager.Instance.PlayEnemySound();

        Instantiate(bloodEfect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
