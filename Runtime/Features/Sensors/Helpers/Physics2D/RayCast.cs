using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazurkaStudio.Utilities.Sensors
{
    public static partial class SensorUtils
    {
        //ALL
        
        /// <summary>
        /// Raycast all along the ray, and return a list of all target found
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> RayCastAll<T>(Vector2 from, Vector2 dir, float distance, LayerMask layerMask, ref RaycastHit2D[] results) where T : Component
        {
            var value = new List<T>();
            
            var size = Physics2D.RaycastNonAlloc(from, dir, results, distance,layerMask);
            
            for (var i = 0; i < size; i++)
            {
                var col = results[i];
                var t = col.collider.GetComponentInParent<T>();
                if (t == null) continue;
                if(!value.Contains(t)) value.Add(t);
            }

            return value;
        }
        
        /// <summary>
        /// Raycast all along the ray, and return a list of all target found
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <param name="conditions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> RayCastAll<T>(Vector2 from, Vector2 dir, float distance, LayerMask layerMask, ref RaycastHit2D[] results,Func<T, bool> conditions) where T : Component
        {
            var value = new List<T>();
            
            var size = Physics2D.RaycastNonAlloc(from, dir, results, distance,layerMask);
            
            for (var i = 0; i < size; i++)
            {
                var col = results[i];
                var t = col.collider.GetComponentInParent<T>();
                if (t == null) continue;
                if(value.Contains(t)) continue;
                if(!conditions.Invoke(t)) continue;
                value.Add(t);
            }

            return value;
        }

        
        //FIRST IN ALL
        
        /// <summary>
        /// Raycast all along the ray and return the first target of type T
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RayCastAllAndReturnFirst<T>(Vector2 from, Vector2 dir, float distance, LayerMask layerMask, ref RaycastHit2D[] results) where T : Component
        {
            var size = Physics2D.RaycastNonAlloc(from, dir, results, distance,layerMask);
            
            for (var i = 0; i < size; i++)
            {
                var col = results[i];
                var t = col.collider.GetComponentInParent<T>();
                if (t == null) continue;
                return t;
            }

            return null;
        }
        
        /// <summary>
        /// Raycast all along the ray and return the first target of type T
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <param name="conditions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RayCastAllAndReturnFirst<T>(Vector2 from, Vector2 dir, float distance, LayerMask layerMask, ref RaycastHit2D[] results, Func<T, bool> conditions) where T : Component
        {
            var size = Physics2D.RaycastNonAlloc(from, dir, results, distance,layerMask);
            var alreadyChecked = new HashSet<T>();
            
            for (var i = 0; i < size; i++)
            {
                var col = results[i];
                var target = col.collider.GetComponentInParent<T>();
                
                if (target == null) continue;
                if (alreadyChecked.Contains(target)) continue;
                
                alreadyChecked.Add(target);
                if (conditions.Invoke(target))  return target;
            }

            return null;
        }


        /// <summary>
        /// Raycast all along the ray and return the first target of type T with raycastHit2D infos
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RaycastHit2D RayCastAllAndReturnFirst<T>(Vector2 from, Vector2 dir, float distance, LayerMask layerMask, ref RaycastHit2D[] results, out T value) where T : Component
        {
            var size = Physics2D.RaycastNonAlloc(from, dir, results, distance,layerMask);
            
            for (var i = 0; i < size; i++)
            {
                var col = results[i];
                value = col.collider.GetComponentInParent<T>();
                if (value == null) continue;
                return col;
            }

            value = null;
            return default;
        }
        
        /// <summary>
        /// Raycast all along the ray and return the first target of type T with raycastHit2D infos
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <param name="value"></param>
        /// <param name="conditions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RaycastHit2D RayCastAllAndReturnFirst<T>(Vector2 from, Vector2 dir, float distance, LayerMask layerMask, ref RaycastHit2D[] results, out T value, Func<T, bool> conditions) where T : Component
        {
            var size = Physics2D.RaycastNonAlloc(from, dir, results, distance,layerMask);
            var alreadyChecked = new HashSet<T>();
           
            for (var i = 0; i < size; i++)
            {
                var hit = results[i];
                var target = hit.collider.GetComponentInParent<T>();
                if (target == null) continue;
                
                if (alreadyChecked.Contains(target)) continue;
                alreadyChecked.Add(target);
                
                if (conditions.Invoke(target))
                {
                    value = target;
                    return hit;
                }
            }

            value = null;
            return default;
        }

        
        
        //SIMPLE
        
        /// <summary>
        /// Simple raycast + try get component in parent
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="layerMask"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RaycastHit2D RayCast<T>(Vector2 from, Vector2 dir, float distance, LayerMask layerMask, out T value) where T : Component
        {
            var hit = Physics2D.Raycast(from, dir, distance,layerMask);
            value = hit? hit.collider.GetComponentInParent<T>() : null;
            return hit;
        }
        
        /// <summary>
        /// Simple raycast + try get component in parent
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="layerMask"></param>
        /// <param name="value"></param>
        /// <param name="conditions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RaycastHit2D RayCast<T>(Vector2 from, Vector2 dir, float distance, LayerMask layerMask, out T value, Func<T, bool> conditions) where T : Component
        {
            var hit = Physics2D.Raycast(from, dir, distance,layerMask);
            var result = hit? hit.collider.GetComponentInParent<T>() : null;
            
            if (result != null)
            {
                if (conditions.Invoke(result))
                {
                    value = result;
                    return hit;
                }
            }
            
            value = null;
            return hit;
        }
        
        
        
        /// <summary>
        /// Simple raycast
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public static RaycastHit2D RayCast(Vector2 from, Vector2 dir, float distance, LayerMask layerMask) => Physics2D.Raycast(from, dir, distance,layerMask);
        
        /// <summary>
        /// Simple raycast
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="conditions"></param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public static RaycastHit2D RayCast(Vector2 from, Vector2 dir, float distance, LayerMask layerMask, Func<RaycastHit2D, bool> conditions)
        {
            var hit = Physics2D.Raycast(from, dir, distance, layerMask);
            return conditions.Invoke(hit) ? hit : default;
        }

        
        
        //GIZMOS
        
        public static void DrawRayCast(Vector2 from,  Vector2 dir, float distance) => Gizmos.DrawLine(from, from + dir * distance);
    }
}
