using System;
using UnityEngine;
using UnityEngine.Pool;

namespace SkyCrush.WSGenerator
{
    public class PoolContainer
    {
        public GameObject Instance { get; private set; }
        public Transform Container { get; private set; }
        public bool CollectionCheck { get; private set; } = false;

        public event Action<GameObject> OnReturnToPool;
        public event Action<GameObject> OnTakeFromPool;

        private ObjectPool<GameObject> _pool;
        private PoolSettings _poolSettings;

        public PoolContainer(PoolSettings poolSettings, Transform parent, PoolObjectInfo poolInfo)
        {
            _poolSettings = poolSettings;
            Instance = poolInfo.instance;

            var poolObject = new GameObject($"Pool Container [{Instance.name}]");
            Container = poolObject.transform;
            Container.SetParent(parent);

            _pool = new ObjectPool<GameObject>(CreatePoolInstance, GetCallback, ReleaseCallback, DestroyCallback, CollectionCheck, poolSettings.DefaultPoolSize);
        }

        public GameObject Get() => _pool.Get();
        public void Release(GameObject poolObject) => _pool.Release(poolObject);

        public void CLear()
        {
            _pool.Dispose();
            _pool.Clear();
        }

        private GameObject CreatePoolInstance()
        {
            var poolObject = GameObject.Instantiate(Instance, Container);
            poolObject.SetActive(false);
            return poolObject;
        }

        private void ReleaseCallback(GameObject poolObject)
        {
            if (poolObject == null) return;

            poolObject.SetActive(false);
            OnReturnToPool?.Invoke(poolObject);
        }

        private void GetCallback(GameObject poolObject)
        {
            if (poolObject == null) return;

            poolObject.SetActive(true);
            OnTakeFromPool?.Invoke(poolObject);
        }

        private void DestroyCallback(GameObject poolObject)
        {
            GameObject.Destroy(poolObject);
        }
    }
}
