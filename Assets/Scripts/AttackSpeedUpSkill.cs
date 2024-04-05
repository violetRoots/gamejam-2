using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(AttackSpeedUpSkill) + "Config", menuName = "Skills/" + nameof(AttackSpeedUpSkill), order = 0)]
public class AttackSpeedUpSkill : SkillConfig
{
    [Range(-100.0f, 100.0f)]
    public float attackSpeedFactorMultiplier;
}
