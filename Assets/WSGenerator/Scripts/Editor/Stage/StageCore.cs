using System;
using UnityEngine;

namespace SkyCrush.WSGenerator
{
    public partial class Stage
    {
        public string Name => name;
        public float Duration => duration;
        public CustomStageData CustomData => customData;
        public GenerateObjectInfo[] GenerateObjects => generateObjects;

        private void OnValidate()
        {
            UpdatePool();
            UpdateCurveDescription();
        }

        public void UpdatePool()
        {
            foreach (var objectInfo in generateObjects) objectInfo.UpdatePool();
        }

        public void UpdateCurveDescription()
        {
            foreach (var objectInfo in generateObjects) objectInfo.UpdateCurveDescription(duration);
        }
    }
}
