using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPanel : View
{
    [SerializeField] private Button startGameButton;

    private LoadManager _loadManager;

    public override void OnCreate()
    {
        base.OnCreate();

        _loadManager = LoadManager.Instance;

        startGameButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        _loadManager.LoadGameplayScene();
    }
}
