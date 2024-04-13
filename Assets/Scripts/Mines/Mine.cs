using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Mine : MonoBehaviour
{
    [SerializeField] private float bombRadius = 2.0f;
    [SerializeField] private int bombDamage = 100;

    public void Init(float timeout)
    {
        StartCoroutine(BombProcess(timeout));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out IDamagable damagable) || damagable is not Enemy) return;

        Bomb();
    }

    private IEnumerator BombProcess(float timeout)
    {
        yield return new WaitForSeconds(timeout);

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
