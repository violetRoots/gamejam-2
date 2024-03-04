using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonoBehaviourBase<InputManager>
{
    public Vector2 MoveDirection { get; private set; }
    public Vector2 RotateDirection { get; private set; }

    private UIManager _uiManager;

    private void Start()
    {
        _uiManager = UIManager.Instance;
    }

    private void Update()
    {
        MoveDirection = _uiManager.MoveJoystick.Direction;
        RotateDirection = _uiManager.RotateJoystick.Direction;
    }
}
