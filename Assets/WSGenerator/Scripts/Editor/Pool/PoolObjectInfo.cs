using System;
using UnityEngine;

namespace SkyCrush.WSGenerator
{
    [Serializable]
    public struct PoolObjectInfo
    {
        public GameObject instance;
        public bool hideInSpawnMenu;
    }
}
