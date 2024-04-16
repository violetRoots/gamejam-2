using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(DragonSkill) + "Config", menuName = "Skills/" + nameof(DragonSkill), order = 0)]
public class DragonSkill : SkillConfig
{
    [Range(0.0f, 100.0f)]
    public float bornChance = 10.0f;
}
