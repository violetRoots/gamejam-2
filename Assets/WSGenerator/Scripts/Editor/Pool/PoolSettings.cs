using UnityEngine;
using NaughtyAttributes;

namespace SkyCrush.WSGenerator
{
    [CreateAssetMenu(fileName = "WSG_PoolSettings", menuName = "WSGenerator/PoolSettings", order = 4)]
    public partial class PoolSettings : ScriptableObject
    {
        private int defaultPoolSize = 100;

        [ReorderableList]
        [SerializeField]
        private PoolObjectInfo[] poolObjectsInfo;
    }
}
