using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviourBase<UIManager>
{
    public Joystick MoveJoystick => moveJoystick;
    public Joystick RotateJoystick => rotateJoystick;

    [SerializeField] private Joystick moveJoystick;
    [SerializeField] private Joystick rotateJoystick;

    [SerializeField] private ExperiencePanel experiencePanel;

    public void UpdateExperiencePanel(int currentLevel, int currentExperience, int levelExperience)
    {
        experiencePanel.UpdateVisual(currentLevel, currentExperience, levelExperience);
    }
}
