using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationGridPoint : MonoBehaviour
{
    public bool IsActive => _deadZoneContacts == 0;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    private int _deadZoneContacts;

    private void Update()
    {
        UpdateVisual(IsActive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.TryGetComponent(out GenerationDeadZone deadZone)) return;

        _deadZoneContacts++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out GenerationDeadZone deadZone)) return;

        _deadZoneContacts--;
    }

    private void UpdateVisual(bool newValue)
    {
        var color = newValue ? Color.green : Color.red;
        _spriteRenderer.color = color;
    }
}
