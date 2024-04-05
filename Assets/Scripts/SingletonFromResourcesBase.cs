using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonFromResourcesBase<T> : SingletonMonoBehaviourBase<T> where T : SingletonFromResourcesBase<T>
{
    private static readonly object InstanceLock = new object();
    private static ResourceRequest _loadingOperation;

    public new static T Instance
    {
        get
        {
            lock (InstanceLock)
            {
                if (_instance == null && !ApplicationState.isQuitting)
                    CreateInstanceSyncLegacy();

                return _instance;
            }
        }
    }

    public static AsyncOperation Load()
    {
        if (_instance != null)
        {
            Debug.LogError($"Instance already has been instantiated for {typeof(T).Name}");
            return null;
        }

        if (_loadingOperation != null)
        {
            Debug.LogError($"Can't start second Loading process for {typeof(T).Name}");
            return null;
        }

        _loadingOperation = Resources.LoadAsync<T>(typeof(T).Name);
        _loadingOperation.completed += LoadingOperationOnComplete;
        return _loadingOperation;
    }

    private static void LoadingOperationOnComplete(AsyncOperation obj)
    {
        Factory(Instantiate(_loadingOperation.asset as T));
        Resources.UnloadAsset(_loadingOperation.asset); // does this necessary?
        _loadingOperation = null;
    }

    private static void CreateInstanceSyncLegacy()
    {
        var name = typeof(T).Name;
        Debug.LogWarning($"Please, use Async Loader for accessing {name}");
        Factory(Instantiate(Resources.Load<T>(name)));
    }
}
