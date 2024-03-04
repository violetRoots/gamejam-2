using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTreeSpritesController : MonoBehaviour
{
    [Button("Update")]
    public void UpdateTrees()
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        for(var i = 0; i < sprites.Length; i++)
        {
            var sprite = sprites[i];
            sprite.sortingLayerName = "Default";
            sprite.sortingOrder = -(int) sprite.transform.position.y - 100000;
            var rand = Random.Range(0, sprites.Length - 1);
            sprite.flipX = rand > sprites.Length / 2;
        }
    }
}
