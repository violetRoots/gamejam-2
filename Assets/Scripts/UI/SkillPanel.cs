using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : View
{
    [SerializeField] private SkillView[] skillViews;
    [SerializeField] private Button applyButton;

    private PauseManager _pauseManager;
    private SkillManager _skillManager;

    private SkillView _selectedSkillView;

    public override void OnCreate()
    {
        base.OnCreate();

        _pauseManager = PauseManager.Instance;
        _skillManager = SkillManager.Instance;

        applyButton.onClick.AddListener(ApplyNewSkill);

        InitSkillViews();
    }

    protected override void OnShow()
    {
        base.OnShow();

        var skills = _skillManager.GetAvailableSkills();
        for(int i = 0; i < skillViews.Length; i++)
        {
            skillViews[i].SetSkill(skills[Mathf.Clamp(i, 0, skills.Length - 1)]);
        }
        SelectSkillView(skillViews.First());

        _pauseManager.Pause();
    }

    protected override void OnHide()
    {
        base.OnHide();

        _pauseManager.UnPause();
    }

    private void InitSkillViews()
    {
        for (int i = 0; i < skillViews.Length; i++)
        {
            skillViews[i].Init(SelectSkillView);
        }
    }

    private void ApplyNewSkill()
    {
        var skill = _selectedSkillView.GetSkill();
        _skillManager.ApplySkill(skill);

        Hide();
    }

    private void SelectSkillView(SkillView skillView)
    {
        _selectedSkillView?.SetSelected(false);

        _selectedSkillView = skillView;

        _selectedSkillView.SetSelected(true);
    }
}
