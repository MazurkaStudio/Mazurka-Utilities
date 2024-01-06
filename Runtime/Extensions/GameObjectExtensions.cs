using UnityEngine;
using Object = UnityEngine.Object;

namespace TheMazurkaStudio.Utilities
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Return object if exist, else return null
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;
        
        public static T GetOrAddComponent<T>(this GameObject self) where T : Component => self.TryGetComponent<T>(out var component) ? component : self.AddComponent<T>();
        
        public static bool HasComponent<T>(this GameObject gameObject) where T : Component => gameObject.GetComponent<T>() != null;
        public static bool HasComponent<T>(this GameObject gameObject, out T component) where T : Component => gameObject.TryGetComponent(out component);
        public static T GetComponentOrNull<T>(this GameObject gameObject) where T : Component => gameObject.TryGetComponent<T>(out var component) ? component : null;
    }
}
