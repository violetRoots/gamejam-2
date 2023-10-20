using UnityEngine;

public abstract class SingletonMonoBehaviourBase<T> : MonoBehaviour where T : SingletonMonoBehaviourBase<T>
{
    protected static T _instance;
    private static readonly object InstanceLock = new object();

    public static T Instance
    {
        get
        {
            lock (InstanceLock)
            {
                _instance = FindFirstObjectByType<T>();

                if (_instance == null && !ApplicationState.isQuitting)
                    Factory(new GameObject().AddComponent<T>());

                return _instance;
            }
        }
    }

    protected virtual void OnApplicationQuit()
    {
        ApplicationState.isQuitting = true;
        if (_instance != null)
            Destroy(_instance);
        _instance = null;
    }

    protected static void Factory(T instance)
    {
        _instance = instance;
        var go = _instance.gameObject;
        go.name = typeof(T).ToString(); // So it has no "(Clone)" in name
        _instance.Init();
    }

    protected virtual void Init()
    {

    }
}

public static class ApplicationState
{
    public static bool isQuitting;
}