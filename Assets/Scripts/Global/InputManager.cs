using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonoBehaviourBase<InputManager>
{
    public bool IsMobileInputEnabled => _enableMobileInput;
    public bool FireButtonPressed { get; private set; }
    public Vector2 MoveDirection { get; private set; }
    public Vector2 RotateDirection { get; private set; }

    [SerializeField]
    private Transform moveContainer;

    private GameManager _gameManager;
    private JoysticksPanel _joysticksPanel;
    private bool _enableMobileInput = false;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _enableMobileInput = _gameManager.GameConfig.enableMobileInput;

        if(_enableMobileInput)
            _joysticksPanel = ViewManager.Instance.Show<JoysticksPanel>();
    }

    private void Update()
    {
        if (_enableMobileInput)
        {
            MobileInput();
        }
        else
        {
            KeyboardMouseInput();
        }
    }

    private void MobileInput()
    {
        Vector2 movedir = Vector2.zero;

        if (_joysticksPanel.MoveJoystick.Direction.magnitude > 0)
        {
            movedir = _joysticksPanel.MoveJoystick.Direction;
        }
        else
        {
            movedir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            movedir = Vector2.ClampMagnitude(movedir, 1.0f);
        }

        MoveDirection = movedir;
        RotateDirection = _joysticksPanel.RotateJoystick.Direction;
    }

    private void KeyboardMouseInput()
    {
        MoveDirection = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1.0f);

        var worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RotateDirection = (worldMousePos - moveContainer.position).normalized;

        FireButtonPressed = Input.GetMouseButton(0);
    }
}
