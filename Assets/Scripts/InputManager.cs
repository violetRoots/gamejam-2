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
        Vector2 movedir;
        if(_uiManager.MoveJoystick.Direction.magnitude > 0)
        {
            movedir = _uiManager.MoveJoystick.Direction;
        }
        else
        {
            movedir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            movedir = Vector2.ClampMagnitude(movedir, 1.0f);
        }

        MoveDirection = movedir;
        RotateDirection = _uiManager.RotateJoystick.Direction;
    }
}
