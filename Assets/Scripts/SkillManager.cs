using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : SingletonMonoBehaviourBase<SkillManager>
{
    [SerializeField] private SkillConfig[] skillConfigs;

    private readonly List<SkillRuntimeInfo> _skills = new List<SkillRuntimeInfo>();
    private readonly List<SkillRuntimeInfo> _appliedSkills = new List<SkillRuntimeInfo>();

    private void Awake()
    {
        for(var i = 0; i < skillConfigs.Length; i++) 
        { 
            var skill = new SkillRuntimeInfo(skillConfigs[i]);
            _skills.Add(skill);
        }
    }

    public SkillRuntimeInfo[] GetAvailableSkills()
    {
        return _skills.Where(skill => !_appliedSkills.Contains(skill)).ToArray();
    }

    public void ApplySkill(SkillRuntimeInfo skill)
    {
        if (skill.Config is ClearSkill) return;

        _appliedSkills.Add(skill);
    }

    public bool IsSkillApplied<T>() where T : SkillConfig
    {
        return _appliedSkills.Find(skill => skill.Config is T) != null;
    }

    public bool IsSkillApplied<T>(out SkillRuntimeInfo resSkill) where T : SkillConfig
    {
        resSkill = _appliedSkills.Find(skill => skill.Config is T);

        return resSkill != null;
    }
}
