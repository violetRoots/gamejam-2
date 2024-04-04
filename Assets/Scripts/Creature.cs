using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour, IDamagable
{
    [Header("Creature Configs")]
    [SerializeField] private int startHealth = 100;
    [SerializeField] private GameObject diedMarker;

    public int Health
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }

    private int _currentHealth;
    private bool _isDied;

    protected virtual void Awake()
    {
        Health = startHealth;
    }

    public abstract bool CanGetDamage();
    public abstract void GetDamage(int damagePoints);
    public void Die()
    {
        if (IsDied()) return;

        DieInternal();
    }

    public virtual void DieInternal()
    {
        //if(diedMarker != null)
        //    Instantiate(diedMarker, transform.position, Quaternion.identity);

        _isDied = true;

        Destroy(gameObject);
    }

    public bool IsDied() => _isDied;
}
