using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HumanShooterController : MonoBehaviour
{
    [SerializeField] private int damage = 100;
    [SerializeField] private float rechargeTime;
    [SerializeField] private float shootCastRadius = 0.5f;
    [SerializeField] private float shootCastDistance = 2.0f;

    protected InputManager _inputManager;

    private float _lastShootTime;

    private void Start()
    {
        _inputManager = InputManager.Instance;
    }

    public virtual void CheckShoot() { }
    protected virtual bool CanShoot()
    {
        bool canShoot = false;
        if (!_inputManager.IsMobileInputEnabled)
        {
            canShoot = _inputManager.FireButtonPressed && Time.time - _lastShootTime >= CalculateRechargeTime();
        }
        else
        {
            var hits = Physics2D.CircleCastAll(transform.position, shootCastRadius, transform.right, shootCastDistance);
            var enemyTransform = hits.Where(hit => hit.collider.TryGetComponent(out Enemy enemy)).Select(hit => hit.transform).FirstOrDefault();

            canShoot = Time.time - _lastShootTime >= CalculateRechargeTime() && enemyTransform != null;
        }

        return canShoot;
    }

    protected virtual void Shoot()
    {
        _lastShootTime = Time.time;
    }

    protected int CalculateDamage()
    {
        return damage;
    }

    protected float CalculateRechargeTime()
    {
        return rechargeTime;
    }
}
