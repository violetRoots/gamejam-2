using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotatableHuman : Human
{
    private InputManager _inputManager;

    private void Start()
    {
        _inputManager = InputManager.Instance;
    }

    protected override float GetSpeed()
    {
        return base.GetSpeed() * _inputManager.RotateDirection.magnitude;
    }
}
