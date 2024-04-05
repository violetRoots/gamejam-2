using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : SingletonMonoBehaviourBase<LoadManager>
{
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    public void LoadGameplayScene()
    {
        SceneManager.LoadScene(_gameManager.GameConfig.gameplaySceneName);
    }
}
