using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnDistance = 10.0f;

    [Space]
    [SerializeField] private Enemy enemy;

    private Enemy _spawnedEnemy;
    private Transform _crowdTransform;

    private void Start()
    {
        _crowdTransform = CrowdController.Instance.MoveContainer.transform;

        SpawnEnemy();
    }

    private void Update()
    {
        var distance = Vector2.Distance(transform.position, _crowdTransform.position);

        if (_spawnedEnemy != null || distance < spawnDistance) return;

        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (_spawnedEnemy != null)
            Destroy(_spawnedEnemy.gameObject);

        _spawnedEnemy = Instantiate(enemy, transform);
    }
}
