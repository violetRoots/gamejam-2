using UnityEngine;

namespace SkyCrush.WSGenerator
{
    public partial class PoolSettings
    {
        public int DefaultPoolSize => defaultPoolSize;
        public ref PoolObjectInfo[] PoolObjectsInfo => ref poolObjectsInfo;
    }
}
