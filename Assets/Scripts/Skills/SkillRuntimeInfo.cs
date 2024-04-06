using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRuntimeInfo
{
    public SkillConfig Config { get; private set; }

    public SkillRuntimeInfo(SkillConfig config)
    { 
        Config = config; 
    }
}
