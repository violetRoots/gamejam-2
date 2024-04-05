using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(RotatableHumanHpUpSkill) + "Config", menuName = "Skills/" + nameof(RotatableHumanHpUpSkill), order = 0)]
public class RotatableHumanHpUpSkill : SkillConfig
{
    [Range(-100.0f, 100.0f)]
    public float hpFactorMultiplier;
}
