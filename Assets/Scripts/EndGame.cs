using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private Transform endDestinationTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Human human)) return;

        CrowdController.Instance.SetNewDestinationForAll(endDestinationTransform);
    }
}
