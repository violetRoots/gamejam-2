using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(AttackUpSkill) + "Config", menuName = "Skills/" + nameof(AttackUpSkill), order = 0)]
public class AttackUpSkill : SkillConfig
{
    [Range(-100.0f, 100.0f)]
    public float attackFactorMultiplier;
}
