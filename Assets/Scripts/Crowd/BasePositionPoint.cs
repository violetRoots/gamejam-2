using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePositionPoint : MonoBehaviour
{
    public float AngleOffset {  get; private set; }

    public void SetAngleOffset(float angleOffset)
    {
        AngleOffset = angleOffset;
    }
}
