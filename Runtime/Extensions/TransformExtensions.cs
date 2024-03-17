using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheMazurkaStudio.Utilities
{
    public static class TransformExtensions
    {
        public static void DestroyChildren(this Transform transform)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
#endif
                for (var i = transform.childCount - 1; i >= 0; i--)
                {
                    Object.Destroy(transform.GetChild(i).gameObject);
                }
                
                return;
#if UNITY_EDITOR
            }

            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(transform.GetChild(i).gameObject);
            }
#endif
        }
        
        public static void PerformOnChildren(this Transform transform, Action<Transform> action)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                action.Invoke(transform.GetChild(i));
            }
        }
        
        public static void PerformOnChildrenReversed(this Transform transform, Action<Transform> action)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                action.Invoke(transform.GetChild(i));
            }
        }
        
        /// <summary>
        /// Reset scale, position, rotation
        /// </summary>
        /// <param name="transform"></param>
        public static void Reset(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void EnableChildren(this Transform transform) => transform.PerformOnChildren(x => x.gameObject.SetActive(true));
        
        public static void DisableChildren(this Transform transform) => transform.PerformOnChildren(x => x.gameObject.SetActive(false));

        /// <summary>
        /// Get all children of the transform
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static IEnumerable<Transform> Children(this Transform transform) => transform.Cast<Transform>();

        /// <summary>
        /// Look at with right direction of the transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="rightDir"></param>
        public static void LookAt2D(this Transform transform, Vector2 rightDir) => transform.rotation = rightDir.LookAt2D();
    }
}
