using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSaveArea : MonoBehaviour
{
    [SerializeField] private int maxHumansCount = 15;

    [MinMaxSlider(0.0f, 20.0f)]
    [SerializeField] private Vector2Int countIconScaleBounds = Vector2Int.up;

    [Space(10)]
    [SerializeField] private Transform countIcon;

    private CrowdController _crowdController;

    private int _currentHumansCount;

    private void Start()
    {
        _crowdController = CrowdController.Instance;
    }

    private void Update()
    {
        var minSize = new Vector2(countIconScaleBounds.x, countIconScaleBounds.x);
        var maxSize = new Vector2(countIconScaleBounds.y, countIconScaleBounds.y);
        countIcon.localScale = Vector2.Lerp(minSize, maxSize, (float) _currentHumansCount / maxHumansCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out StaticHuman staticHuman)) return;

        _crowdController.RemoveHuman(staticHuman);
        staticHuman.Save();

        staticHuman.SetDestinationPosition(transform.position);

        _currentHumansCount++;
    }
}
