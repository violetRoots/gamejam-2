using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkyCrush.WSGenerator
{
    public class GridManager : MonoBehaviour
    {
        private GeneratorGrid _grid;
        private GenerationGridPoint[] _generationGridPoints;

        public void Init(GeneratorGrid generatorGrid)
        {
            _grid = generatorGrid;

            _generationGridPoints = _grid.GetGenerationPoints();
        }

        public GenerationGridPoint GetRandomActiveGenerationPoint()
        {
            var points = GetActiveGenerationPoints();
            if(points.Length == 0) return null;
            else return points[Random.Range(0, points.Length - 1)];
        }

        public GenerationGridPoint[] GetActiveGenerationPoints()
        {
            return _generationGridPoints.Where(point => point.IsActive).ToArray();
        }
    }
}
