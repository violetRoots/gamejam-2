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

    protected virtual void Awake()
    {
        Health = startHealth;
    }

    public abstract bool CanGetDamage();
    public abstract void GetDamage(int damagePoints);
    public virtual void Die()
    {
        if(diedMarker != null)
            Instantiate(diedMarker, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

}
