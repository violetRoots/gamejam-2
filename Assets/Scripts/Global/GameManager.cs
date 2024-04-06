using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonFromResourcesBase<GameManager>
{
    public GameConfigs GameConfig => gameConfigs;
    public RuntimeGameData RuntimeGameData => _runtimeGameData;

    [SerializeField] private GameConfigs gameConfigs;

    private RuntimeGameData _runtimeGameData;

    private void Awake()
    {
        _runtimeGameData = new RuntimeGameData();
    }
}
