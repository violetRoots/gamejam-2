using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour, IDamagable
{
    [Header("Creature Configs")]
    [SerializeField] protected int startHealth = 100;
    [SerializeField] private GameObject diedMarker;

    public int Health
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }

    private int _currentHealth;
    private bool _isDead;

    protected virtual void OnEnable()
    {
        SetStartHealth();
        SetDead(false);
    }

    public abstract bool CanGetDamage();
    public abstract void GetDamage(int damagePoints);
    public void Die()
    {
        if (IsDead()) return;

        DieInternal();
    }

    public virtual void DieInternal()
    {
        //if(diedMarker != null)
        //    Instantiate(diedMarker, transform.position, Quaternion.identity);

        SetDead(true);

        Destroy(gameObject);
    }

    public void SetDead(bool isDead)
    {
        _isDead = isDead;
    }

    public bool IsDead() => _isDead;

    protected virtual void SetStartHealth()
    {
        Health = startHealth;
    }
}
