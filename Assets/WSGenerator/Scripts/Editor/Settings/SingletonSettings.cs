using UnityEngine;
using SkyCrush.Utility;

namespace SkyCrush.WSGenerator
{
    public class SingletonSettings<T> : SingletonSOEditorOnly<T> where T : SingletonSOEditorOnly<T>
    {
#if UNITY_EDITOR
        private const string NewLoadPath = "WSGenerator/Settings/";

        public static new T Instance
        {
            get
            {
                LoadPath = NewLoadPath;
                return SingletonSOEditorOnly<T>.Instance;
            }
        }
#endif
    }
}
