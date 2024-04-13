using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaticPositionPoint : BasePositionPoint
{
    [SerializeField] private TextMeshPro _textMeshPro;

    public void SetText(string text)
    {
        _textMeshPro.text = text;
    }
}