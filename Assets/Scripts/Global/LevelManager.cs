using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviourBase<LevelManager>
{
    [SerializeField] private int[] levelsExperiencePoints = new int[1] { 100 };

    private int LevelExperiencePoints => levelsExperiencePoints[Mathf.Clamp(_currentLevel, 0, levelsExperiencePoints.Length - 1)];

    private ViewManager _viewManager;

    private int _currentLevel;
    private int _currentLevelExperiencePoints;

    private ExperiencePanel _experiencePanel;
    private SkillPanel _skillPanel;

    private void Start()
    {
        _viewManager = ViewManager.Instance;
        _experiencePanel = _viewManager.Show<ExperiencePanel>();
        _skillPanel = _viewManager.Get<SkillPanel>();

        UpdateUIVisual();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Equals))
        {
            LevelUp();
            UpdateUIVisual();
        }
    }


    public void AddExperiencePoints(int value)
    {
        _currentLevelExperiencePoints += value;
        if (_currentLevelExperiencePoints >= LevelExperiencePoints)
        {
            LevelUp();
        }

        UpdateUIVisual();
    }

    private void LevelUp()
    {
        _currentLevelExperiencePoints = 0;
        _currentLevel++;

        _skillPanel.Show();
    }

    private void UpdateUIVisual()
    {
        _experiencePanel.UpdateVisual(_currentLevel, _currentLevelExperiencePoints, LevelExperiencePoints);
    }
}
