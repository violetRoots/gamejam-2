using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoEndGame : MonoBehaviour
{
    [SerializeField] private Transform endDestinationTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out DemoHuman human)) return;

        DemoCrowdController.Instance.SetNewDestinationForAll(endDestinationTransform);
    }
}
