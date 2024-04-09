using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

namespace SkyCrush.WSGenerator
{
    public partial class Generator : MonoBehaviour
    {
        [SerializeField] 
        private Settings settings;

        [SerializeField]
        private GeneratorGrid grid;

        [Space(10)]
        [SerializeField] 
        private Sequence sequence;
        [SerializeField] 
        private bool autoPlay = true;

        [ShowIf(nameof(IsInitilized))]
        [Space(10)]
        [Header("Current Stage")]
        [ReadOnly]
        [SerializeField] 
        private string stageName;
        [ShowIf(nameof(IsInitilized))]
        [ProgressBar(100)]
        [SerializeField] 
        private float stageProcess;
    }
}
