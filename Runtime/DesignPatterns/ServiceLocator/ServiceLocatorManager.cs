using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    public class ServiceLocatorManager
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        
        public IEnumerable<object> RegisteredServices => _services.Values;

        public ServiceLocatorManager Register<T>(T service)
        {
            var type = typeof(T);

            if (!_services.TryAdd(type, service)) Debug.LogError($"Service of type {type.FullName} already registered");
            return this;
        }
        
        public ServiceLocatorManager Register(Type type, object service)
        {
            if (!type.IsInstanceOfType(service)) throw new ArgumentException("type of service do no match type of service interface", nameof(service));
            
            if (!_services.TryAdd(type, service)) Debug.LogError($"Service of type {type.FullName} already registered");
            
            return this;
        }

        
        public bool TryGet<T>(out T service) where T : class
        {
            var type = typeof(T);

            if (_services.TryGetValue(type, out var serviceObj))
            {
                service = serviceObj as T;
                return true;
            }

            service = null;
            return false;
        }
        
        public T Get<T>() where T : class
        {
            var type = typeof(T);

            if (_services.TryGetValue(type, out var service)) return service as T;
            throw new ArgumentException($"Service of type {type.FullName} not registered");
        }
    }
}
