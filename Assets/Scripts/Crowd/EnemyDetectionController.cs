using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDetectionController : MonoBehaviour
{
    [SerializeField] private Transform shootOriginPoint;

    private readonly HashSet<Enemy> _enemies = new();

    private void Update()
    {
        StartCoroutine(ClearEnemiesOnEndOfFrame());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Enemy enemy)) return;

        _enemies.Add(enemy);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Enemy enemy)) return;

        _enemies.Remove(enemy);
    }

    public Enemy GetNearestEnemy()
    {
        return _enemies.OrderBy(enemy => Vector2.Distance(shootOriginPoint.position, enemy.transform.position)).FirstOrDefault();
    }

    private IEnumerator ClearEnemiesOnEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        _enemies.RemoveWhere(enemy => enemy == null);
    }
}
