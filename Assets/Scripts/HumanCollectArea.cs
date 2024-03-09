using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class HumanCollectArea : MonoBehaviour
{
    private CrowdController _crowdController;

    private void Start()
    {
        _crowdController = CrowdController.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Human human)) return;

        if (!_crowdController.HasHuman(human))
        {
            _crowdController.AddHuman(human);
        }
    }
}
