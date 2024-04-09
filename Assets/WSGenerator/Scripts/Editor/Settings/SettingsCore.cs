using UnityEngine;
using UnityEditor;
using NaughtyAttributes;

namespace SkyCrush.WSGenerator
{
    public partial class Settings
    {
        public float FrequencySecondsPerUnit => frequencySecondsPerUnit;
        public float MinFrequencyGenerationValue => minFrequencyGenerationValue;
        public float MaxFrequencyGenerationValue => maxFrequencyGenerationValue;
        public PoolSettings PoolSettings => poolSettings;

#if UNITY_EDITOR
        public static void Select()
        {
            Selection.activeObject = Instance;
        }
#endif
    }
}
