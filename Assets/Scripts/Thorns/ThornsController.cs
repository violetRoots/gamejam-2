using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsController : MonoBehaviour
{
    [SerializeField] private float spawnTimeout = 0.25f;
    [SerializeField] private int damage = 50;
    [SerializeField] private int thornsCount = 7;
    [SerializeField] private float spawnDistance = 2.0f;

    [Space]
    [SerializeField] private Thorn thornPrefab;

    private float _lastSpawnTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!collision.TryGetComponent(out Enemy enemy)) return;

        if (CanSpawnThorns())
            SpawnThorns();
    }

    private bool CanSpawnThorns()
    {
        return Time.time - _lastSpawnTime >= spawnTimeout;
    }

    private void SpawnThorns()
    {
        var angle = 360.0f / thornsCount;
        for (int i = 0; i < thornsCount; i++)
        {
            var dir = Quaternion.Euler(0.0f, 0.0f, angle * i) * Vector2.right;
            var pos = dir * spawnDistance;
            SpawnThorn(dir, pos);
        }
        _lastSpawnTime = Time.time;
    }

    private void SpawnThorn(Vector2 postition, Vector2 direction)
    {
        var thorn = Instantiate(thornPrefab, transform.position + (Vector3) postition, Quaternion.identity);
        thorn.Init(direction, damage);
    }
}
