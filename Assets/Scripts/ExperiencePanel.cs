using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperiencePanel : View
{
    [SerializeField] private Image filler;
    [SerializeField] private TextMeshProUGUI levelText;

    public void UpdateVisual(int currentLevel, int currentExperience, int levelExperience)
    {
        levelText.text = $"LVL {currentLevel}";
        var scaleValue = (float) currentExperience / levelExperience;
        filler.transform.localScale = new Vector3(scaleValue, 1.0f, 1.0f);
    }
}
