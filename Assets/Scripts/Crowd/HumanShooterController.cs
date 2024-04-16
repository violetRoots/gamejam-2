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

    protected SkillManager _skillManager;

    private float _lastShootTime;

    private void Start()
    {
        _skillManager = SkillManager.Instance;
    }

    public virtual void CheckShoot() { }
    protected virtual bool CanShoot()
    {
        var hits = Physics2D.CircleCastAll(transform.position, shootCastRadius, transform.right, shootCastDistance);
        var enemyTransform = hits.Where(hit => hit.collider.TryGetComponent(out Enemy enemy)).Select(hit => hit.transform).FirstOrDefault();

        return Time.time - _lastShootTime >= CalculateRechargeTime() && enemyTransform != null;
    }

    protected virtual void Shoot()
    {
        _lastShootTime = Time.time;
    }

    protected int CalculateDamage()
    {
        float newBulletDamage = damage;
        if (_skillManager.IsSkillApplied(out AttackUpSkill attackUpSkillConfig))
            newBulletDamage += damage * attackUpSkillConfig.attackFactorMultiplier / 100.0f;

        return (int)newBulletDamage;
    }

    protected float CalculateRechargeTime()
    {
        var newRechargeTime = rechargeTime;
        if (_skillManager.IsSkillApplied(out AttackSpeedUpSkill attackSpeedUpSkillConfig))
            newRechargeTime += rechargeTime * 0.99f * attackSpeedUpSkillConfig.attackSpeedFactorMultiplier / 100.0f;

        return newRechargeTime;
    }
}
