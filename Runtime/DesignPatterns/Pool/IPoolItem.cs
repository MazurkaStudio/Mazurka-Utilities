using System;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// Implement this pool interface to keep a reference to the pool in the pool item itself
    /// Easy to use.
    /// You can inherit BasePoolItem class to easy use
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPoolItem<T> where T : Component
    {
        public Pool<T> LastPool { get; set; }
        /// <summary>
        /// Disable object (or destroy if pool is null) and pu it back to the pool
        /// </summary>
        public void ReturnToLastPool();
        /// <summary>
        /// Call by pool when spawn item
        /// </summary>
        public void OnSpawnFromPool();
        /// <summary>
        /// A reference to the pool item (this as T)
        /// </summary>
        public T PoolItemValue { get; }
    }

    // => THIS IS HOW TO FULL IMPLEMENT A POOL INTERFACE, has you can see there is check on disable to avoid double return to pool on disable
    
    public abstract class BasePoolItem<T> : MonoBehaviour, IPoolItem<T> where T : Component
    {
        [SerializeField] private bool _setPoolAsParentWhenReturn;
        public Pool<T> LastPool { get; set; }

        /// <summary>
        /// Already try to return to pool this lifecycle
        /// </summary>
        private bool hasReturnToPool = false;
        
        /// <summary>
        /// Is currently in disable state (can't set parent when disable state is active)
        /// </summary>
        private bool disableState = false;

        private void OnDisable()
        {
            disableState = true;
            Despawn();
            ReturnToLastPool();
        }
        private void OnEnable() => Spawn();

        /// <summary>
        /// Call this instead disable or destroy
        /// </summary>
        public void ReturnToLastPool()
        {
            if (hasReturnToPool) return;
            
            hasReturnToPool = true;

            if (LastPool != null)
            {
                if (!disableState && _setPoolAsParentWhenReturn) PoolItemValue.transform.SetParent(LastPool.PoolFolder);
                LastPool.ReturnPoolItem(PoolItemValue);
            }
            else Destroy(PoolItemValue.gameObject);
        }
        public void OnSpawnFromPool()
        {
            disableState = false;
            hasReturnToPool = false;
            BeforeSpawn();
        }

        //Use this instead awake enable disable
        protected abstract void BeforeSpawn();
        protected abstract void Spawn();
        protected abstract void Despawn();

        public T PoolItemValue => this as T;
    }
}
