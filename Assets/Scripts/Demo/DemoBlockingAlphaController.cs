using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBlockingAlphaController : MonoBehaviour
{
    [SerializeField] private float alpha;

#if UNITY_EDITOR
    private void OnValidate()
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();

        foreach(var sprite in sprites)
        {
            var c = sprite.color;
            c.a = alpha;
            sprite.color = c;
        }
    }
#endif
}
