using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillConfig : ScriptableObject
{
    [Header("General")]
    public Sprite icon;
    public string description;
}
