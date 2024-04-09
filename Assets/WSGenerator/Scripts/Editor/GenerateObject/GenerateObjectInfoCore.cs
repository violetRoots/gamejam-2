using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace SkyCrush.WSGenerator
{
    public partial class GenerateObjectInfo
    {
        public const float CurveRange = 10.0f;

        public PoolObjectInfo Pool => poolObjectValue;
        public AnimationCurve FrequencyCurve => frequencyCurve;
        public string InstanceName => instanceName;
        public GameObject Instance => poolObjectValue.instance;

        private string[] InstanceNames => _poolDictionary.Keys.ToArray();

        private Dictionary<string, PoolObjectInfo> _poolDictionary = new Dictionary<string, PoolObjectInfo> { { "no name", new PoolObjectInfo() } };


        public void UpdatePool()
        {
#if UNITY_EDITOR
            var poolsInfo = Settings.Instance.PoolSettings.PoolObjectsInfo;
            _poolDictionary = new Dictionary<string, PoolObjectInfo>();

            for (var i = 0; i < poolsInfo.Length; i++)
            {
                if (poolsInfo[i].instance == null || poolsInfo[i].hideInSpawnMenu) continue;

                _poolDictionary.Add(poolsInfo[i].instance.name, poolsInfo[i]);
            }

            if (_poolDictionary.TryGetValue(instanceName, out PoolObjectInfo poolObjectInfo))
            {
                poolObjectValue = poolObjectInfo;
            }
#endif
        }

        public void UpdateCurveDescription(float duration)
        {
#if UNITY_EDITOR
            x = $"time: 1 unit = duration / {CurveRange} ({(float) (duration / CurveRange)} sec)";
            y = $"frequency: 1 unit = 1 obj / {Settings.Instance.FrequencySecondsPerUnit} sec ({(float)(1 / Settings.Instance.FrequencySecondsPerUnit)} obj/sec)";

            var count = 0;
            var time = 0.0f;
            while(time < duration)
            {
                var process = Mathf.Clamp01((float)(time / duration));
                var frequency = (float) (frequencyCurve.Evaluate(process * CurveRange) / Settings.Instance.FrequencySecondsPerUnit);
                var clampFrequency = Mathf.Clamp(frequency, Settings.Instance.MinFrequencyGenerationValue, Settings.Instance.MaxFrequencyGenerationValue);

                count++;
                time += (float)(1 / clampFrequency);
            }

            var countSize = count.ToString().Length;
            var correction = countSize < 3 ? 1 : Mathf.Pow(10, (countSize-2));
            objectsCount = $"{count}+-{correction}";
#endif
        }
    }
}
