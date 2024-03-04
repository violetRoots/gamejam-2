using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DemoEnemy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.TryGetComponent(out DemoHuman human)) return;

        DemoCrowdController.Instance.TryKill(human, false);
    }
}
