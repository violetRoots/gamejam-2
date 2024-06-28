using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameConfigs), menuName = nameof(GameConfigs), order = 0)]
public class GameConfigs : ScriptableObject
{
    [Header("Input")]
    public bool enableMobileInput = true;

    [Header("Scenes")]
    public string mainMenuSceneName = "MainMenu";
    public string gameplaySceneName = "MobileTest";
}
