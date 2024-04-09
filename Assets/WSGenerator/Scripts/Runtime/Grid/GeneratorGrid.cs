using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorGrid : MonoBehaviour
{
    [SerializeField] private Vector2 leftDownCorner;
    [SerializeField] private Vector2 rightUpperCorner;
    [SerializeField] private float gridDistance = 0.5f;

    [Space(10)]
    [SerializeField] private GenerationGridPoint generationPointPrefab;
    [SerializeField] private Transform gridContainer;

    [SerializeField] private List<GenerationGridPoint> _generationPoints = new List<GenerationGridPoint>();

    [Button]
    private void ClearGrid()
    {
        _generationPoints.Clear();

        for(var i = gridContainer.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(gridContainer.GetChild(i).gameObject);
        }
    }

    [Button]
    private void InitGrid()
    {
        var leftDownCornerByTransform = transform.position + (Vector3)leftDownCorner;
        var rightUpperCornerByTransform = transform.position + (Vector3)rightUpperCorner;

        var rowsCount = (int)(rightUpperCornerByTransform.x - leftDownCornerByTransform.x) / gridDistance;
        var columnsCount = (int)(rightUpperCornerByTransform.y - leftDownCornerByTransform.y) / gridDistance;

        for(var i = 0;i < rowsCount;i++)
        {
            for(var j = 0;j < columnsCount; j++)
            {
                var pos = leftDownCornerByTransform + new Vector3(gridDistance * i, gridDistance * j);
                var point = Instantiate(generationPointPrefab, pos, Quaternion.identity, gridContainer);

                _generationPoints.Add(point);
            }
        }
    }

    public GenerationGridPoint[] GetGenerationPoints()
    {
        return _generationPoints.ToArray();
    }
}
