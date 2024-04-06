using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class HumanCollectArea : MonoBehaviour
{
    [SerializeField] private float startCastRadius = 5.0f;

    private CrowdController _crowdController;

    private void Start()
    {
        _crowdController = CrowdController.Instance;

        AddHumansByCast();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Human human)) return;

        if (!_crowdController.HasHuman(human))
        {
            _crowdController.AddHuman(human);
        }
    }

    private void AddHumansByCast()
    {
        var hits = Physics2D.CircleCastAll(transform.position, startCastRadius, Vector2.zero);
        foreach (var hit in hits)
        {
            if (!hit.collider.TryGetComponent(out Human human)) continue;

            _crowdController.AddHuman(human);
        }
    }
}
