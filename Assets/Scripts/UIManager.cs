using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviourBase<UIManager>
{
    public FixedJoystick MoveJoystick => moveJoystick;
    public FixedJoystick RotateJoystick => rotateJoystick;

    [SerializeField] private FixedJoystick moveJoystick;
    [SerializeField] private FixedJoystick rotateJoystick;
}
