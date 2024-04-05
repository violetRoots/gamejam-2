using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    [SerializeField] private Image skillIconImage;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private Image selectedImage;
    [SerializeField] private Button chooseButton;

    private SkillRuntimeInfo _skill;

    public void Init(UnityAction<SkillView> chooseButtonAction)
    {
        SetSelected(false);

        chooseButton.onClick.RemoveAllListeners();
        chooseButton.onClick.AddListener(() => chooseButtonAction(this));
    }

    public void SetSkill(SkillRuntimeInfo skill)
    {
        _skill = skill;

        skillIconImage.sprite = _skill.Config.icon;
        skillDescription.text = _skill.Config.description;
    }

    public SkillRuntimeInfo GetSkill()
    {
        return _skill;
    }

    public void SetSelected(bool value)
    {
        selectedImage.gameObject.SetActive(value);
    }
}
