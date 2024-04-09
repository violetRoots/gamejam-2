using System;
using UnityEngine;
using NaughtyAttributes;

namespace SkyCrush.WSGenerator
{

    [Serializable]
    public partial class GenerateObjectInfo
    {
        [Header("Instance")]

        [Dropdown(nameof(InstanceNames))]
        [OnValueChanged(nameof(UpdatePool))]
        [AllowNesting]
        [SerializeField]
        private string instanceName;
        [ReadOnly]
        [AllowNesting]
        [SerializeField]
        private PoolObjectInfo poolObjectValue;

        [Header("Generate Process")]

        [AllowNesting]
        [SerializeField]
        [CurveRange(0, 0, CurveRange, CurveRange, EColor.Red)]
        private AnimationCurve frequencyCurve;

        [AllowNesting]
        [ReadOnly]
        [SerializeField]
        private string x, y;

        [AllowNesting]
        [ReadOnly]
        [SerializeField]
        private string objectsCount;
    }
}

