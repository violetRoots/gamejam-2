using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviourBase<LevelManager>
{
    [SerializeField] private int[] levelsExperiencePoints = new int[1] { 100 };

    private int LevelExperiencePoints => levelsExperiencePoints[Mathf.Clamp(_currentLevel, 0, levelsExperiencePoints.Length - 1)];

    private UIManager _uIManager;

    private int _currentLevel;
    private int _currentLevelExperiencePoints;

    private void Start()
    {
        _uIManager = UIManager.Instance;

        UpdateUIVisual();
    }

    public void AddExperiencePoints(int value)
    {
        _currentLevelExperiencePoints += value;
        if (_currentLevelExperiencePoints >= LevelExperiencePoints)
        {
            _currentLevelExperiencePoints = 0;
            _currentLevel++;
        }

        UpdateUIVisual();
        //Debug.Log($"Current level {_currentLevel}: {_currentLevelExperiencePoints}/{levelExperience}");
    }

    private void UpdateUIVisual()
    {
        _uIManager.UpdateExperiencePanel(_currentLevel, _currentLevelExperiencePoints, LevelExperiencePoints);
    }
}
