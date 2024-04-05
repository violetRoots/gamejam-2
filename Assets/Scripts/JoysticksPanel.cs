using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoysticksPanel : View
{
    public Joystick MoveJoystick => moveJoystick;
    public Joystick RotateJoystick => rotateJoystick;

    [SerializeField] private Joystick moveJoystick;
    [SerializeField] private Joystick rotateJoystick;
}
