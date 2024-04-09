using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkyCrush.WSGenerator
{
    public class PoolManager
    {
        public event Action<GameObject> OnReturnToPools;
        public event Action<GameObject> OnTakeFromPools;

        private const string PoolsContainersParentName = "Pools";

        private PoolSettings _poolSettings;
        private Transform _poolsContainersParent;
        private List<PoolContainer> _poolContainers = new List<PoolContainer>();

        public void Init(Settings settings, Sequence sequence, Transform transform)
        {
            _poolSettings = settings.PoolSettings;

            _poolsContainersParent = new GameObject(PoolsContainersParentName).transform;
            _poolsContainersParent.SetParent(transform);

            foreach (var poolInfo in _poolSettings.PoolObjectsInfo)
            {
                CreateContainer(poolInfo);
            }
        }

        public PoolContainer GetPoolContainer(string name, bool isFormat = false)
        {
            PoolContainer res = null;

            foreach(var poolContainer in _poolContainers)
            {
                var instanceName = poolContainer.Instance.name;

                if (isFormat)
                {
                    instanceName = string.Format("{0}(Clone)", instanceName);
                }

                if (name != instanceName) continue;

                res = poolContainer;
            }

            return res;
        }

        private PoolContainer CreateContainer(PoolObjectInfo poolInfo)
        {
            var container = new PoolContainer(_poolSettings, _poolsContainersParent, poolInfo);

            container.OnTakeFromPool += GetCallback;
            container.OnReturnToPool += ReleaseCallback;

            _poolContainers.Add(container);

            return container;
        }

        private void GetCallback(GameObject poolObject)
        {
            OnTakeFromPools?.Invoke(poolObject);
        }

        private void ReleaseCallback(GameObject poolObject)
        {
            OnReturnToPools?.Invoke(poolObject);
        }

        public void Clear()
        {
            foreach (var poolContainer in _poolContainers)
            {
                poolContainer.OnTakeFromPool -= GetCallback;
                poolContainer.OnReturnToPool -= ReleaseCallback;

                GameObject.Destroy(poolContainer.Container.gameObject);

                poolContainer.CLear();
            }

            _poolContainers.Clear();
        }
    }
}
