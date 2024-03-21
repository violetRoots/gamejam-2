using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public int Health { get; set; }

    public void Damage(int damagePoints);
    public void Die();
    public bool CanDamage();
}
