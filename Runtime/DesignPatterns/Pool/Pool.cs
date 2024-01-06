using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// - Basic pool system for any components types
    /// - Return item to pool instead disable or destroy it.
    /// - Pool should be dispose when owner disable.
    /// - Pool should instanced when owner enable.
    /// - When disposed, all items in pool are destroyed. You should manage yourself out of pool items.
    /// - This pool system was not designed for huge or fast features but more has a design pattern for game structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pool<T>  where T : Component
    {
        //todo : find a wau to put back item in folder after disable (unity dont allow set parent on disable)
        
        public Pool(T itemToPool, int initialPoolSize, Transform poolFolder, bool shouldExtend = true, int poolExtendRange = 1)
        {
            this.itemToPool = itemToPool;
            this.initialPoolSize = initialPoolSize;
            this.poolFolder = poolFolder;
            this.shouldExtend = shouldExtend;
            this.poolExtendRange = poolExtendRange;
            isDisposed = false;
            
            PopulatePool();
        }
        
        private Queue<T> poolItems;
        private readonly bool shouldExtend;
        private readonly int initialPoolSize;
        private readonly Transform poolFolder;
        private readonly int poolExtendRange;
        private readonly T itemToPool;
        private bool isDisposed;
        public Transform PoolFolder => poolFolder;
        
        /// <summary>
        /// Return an item from the pool
        /// Use on enable to initialize
        /// </summary>
        /// <param name="item"></param>
        /// <param name="executeBeforeSetActive"></param>
        /// <returns></returns>
        public bool TryGetPoolItem(out T item, Action<T> executeBeforeSetActive = null)
        {
            item = null;
            if (isDisposed) return false;
            if (poolItems.Count < 1)
            {
                if (!shouldExtend) return false;
                item = ExtendAndReturnPoolItem();
            }
            else
            {
                item = poolItems.Dequeue();
                if (item == null) item = CreateAndReturnPoolItem();
            }

            if (item is IPoolItem<T> i) i.OnSpawnFromPool();
            
            executeBeforeSetActive?.Invoke(item);
            
            item.gameObject.SetActive(true);
            return true;
        }

        private void PopulatePool()
        {
            poolItems = new Queue<T>();

            if (itemToPool == null)
            {
                Debug.LogError("Can't found poolItem to pool");
                return;
            }
            if (poolFolder == null) 
            {
                Debug.LogError("Can't found pool container to pool");
                return;
            }
            
            for (var i = 0; i < initialPoolSize; i++)
            {
                var poolItem = Object.Instantiate(itemToPool, poolFolder);
                    
                if (poolItem is IPoolItem<T> p)
                {
                    p.LastPool = this;
                    p.ReturnToLastPool();
                }
                else AddToPool(poolItem);
            }
        }

        private T CreateAndReturnPoolItem()
        {
            TryCreatePoolItem();
            return poolItems.Dequeue();
        }

        private T ExtendAndReturnPoolItem()
        {
            for (var i = 0; i < poolExtendRange; i++)  TryCreatePoolItem();
            return poolItems.Dequeue();
        }
        
        private void TryCreatePoolItem()
        {
            if (itemToPool == null)
            {
                Debug.LogError("Can't found poolItem to pool");
                return;
            }
            if (poolFolder == null) 
            {
                Debug.LogError("Can't found pool container to pool");
                return;
            }

            CreatePoolItem();
        }
        private void CreatePoolItem()
        {
            var poolItem = Object.Instantiate(itemToPool, poolFolder);
            AddToPool(poolItem);
        }
        
        private void AddToPool(T item)
        {
            if (item is IPoolItem<T> i) i.LastPool = this;
            item.gameObject.SetActive(false);
            poolItems.Enqueue(item);
        }
        
        /// <summary>
        /// Disable object and add it to queue
        /// Destroy it if pool is disposed
        /// </summary>
        /// <param name="item"></param>
        public void ReturnPoolItem(T item)
        {
            if (poolItems.Contains(item)) return;
            if (!isDisposed) AddToPool(item);
            else Object.Destroy(item.gameObject);
        }

        /// <summary>
        /// Destroy all elements in the pool
        /// Destroy any next return pool item
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            var index = poolItems.Count;
            for (var i = 0; i < index; i++) Object.Destroy(poolItems.Dequeue());
            poolItems.Clear();
        }
    }
}
