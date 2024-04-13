using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Mine : MonoBehaviour
{
    [SerializeField] private float bombTimeout = 1.0f;
    [SerializeField] private float bombRadius = 2.0f;
    [SerializeField] private int bombDamage = 100;

    private void Start()
    {
        StartCoroutine(BombProcess());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out IDamagable damagable) || damagable is not Enemy) return;

        Bomb();
    }

    private IEnumerator BombProcess()
    {
        yield return new WaitForSeconds(bombTimeout);

        Bomb();
    }

    private void Bomb()
    {
        var hits = Physics2D.CircleCastAll(transform.position, bombRadius, Vector2.zero);
        foreach (var hit in hits)
        {
            if (!hit.collider.TryGetComponent(out IDamagable damagable) || damagable is not Enemy) continue;

            if (damagable.CanGetDamage())
                damagable.GetDamage(bombDamage);
        }

        Destroy(gameObject);
    }
}
