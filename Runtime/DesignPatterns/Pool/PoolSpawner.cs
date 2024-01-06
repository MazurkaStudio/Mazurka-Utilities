using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    public abstract class PoolSpawner<T> : MonoBehaviour where T: Component
    {
        [SerializeField, BoxGroup("Pool Settings")] private T _prefabToSpawn;
        [SerializeField, BoxGroup("Pool Settings")] private int _poolSize;
        [SerializeField, BoxGroup("Pool Settings")] private bool _poolShouldExtend;
        [SerializeField, ShowIf("_poolShouldExtend"), BoxGroup("Pool Settings")] private int _poolExtendRange = 1;
        
        protected virtual bool CanSpawn => isEnable;
        private bool isEnable;
        private Pool<T> _poolItems;
        
        protected virtual void OnEnable()
        {
            _poolItems = new Pool<T>(_prefabToSpawn, _poolSize, transform, _poolShouldExtend, _poolExtendRange);
            isEnable = true;
        }

        public bool TrySpawnItem(out T item, Action<T> executeBeforeSetActive = null)
        {
            item = null;
            return CanSpawn && _poolItems.TryGetPoolItem(out item, executeBeforeSetActive);
        }

        public void ReturnToPool(T item)
        {
            if(isEnable) _poolItems.ReturnPoolItem(item);
            else Destroy(item.gameObject);
        }

        protected virtual void OnDisable()
        {
            isEnable = false;
            _poolItems.Dispose();
        }
    }
}
