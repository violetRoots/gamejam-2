using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoInputManager : SingletonMonoBehaviourBase<DemoInputManager>
{
    public event Action OnLeftMouseButtonDown;
    public event Action OnEcsButtonDown;
    public bool GameplayInputEnabled { get; set; }
    public Vector3 MovementInputVector { get; private set; }
    public Vector3 WorldMousePos { get; private set; }

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        UpdateEscInput();

        if (!GameplayInputEnabled) return; 

        MovementInputVector = GetMovementInput();
        WorldMousePos = GetWorldMousePos();

        UpdateMouseInput();
    }

    private Vector2 GetMovementInput()
    {
        var xInput = Input.GetAxis("Horizontal");
        var yInput = Input.GetAxis("Vertical");

        return new Vector3(xInput, yInput).normalized;
    }

    private Vector3 GetWorldMousePos()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = _camera.transform.position.z;
        return _camera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void UpdateMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
            OnLeftMouseButtonDown?.Invoke();
    }

    private void UpdateEscInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnEcsButtonDown?.Invoke();
    }
}
