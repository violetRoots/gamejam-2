using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(RebornUpSkill) + "Config", menuName = "Skills/" + nameof(RebornUpSkill), order = 0)]
public class RebornUpSkill : SkillConfig
{
    [Range(-100.0f, 100.0f)]
    public float experienceFactorMultiplier;
}
