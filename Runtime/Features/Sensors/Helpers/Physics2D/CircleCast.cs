using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheMazurkaStudio.Utilities.Sensors
{
    public static partial class SensorUtils
    {
        //ALL
        
        public static List<T> CircleCastAll<T>(Vector2 from, Vector2 dir, float radius, float distance, LayerMask layerMask, ref RaycastHit2D[] results) where T : Component
        {
            var value = new List<T>();
            
            var size = Physics2D.CircleCastNonAlloc(from, radius, dir, results, distance, layerMask);
            
            for (int i = 0; i < size; i++)
            {
                var col = results[i];

                var t = col.collider.GetComponentInParent<T>();
                if (t == null) continue;
                if(!value.Contains(t)) value.Add(t);
            }
            
            return value;
        }
        
        public static List<T> CircleCastAll<T>(Vector2 from, Vector2 dir, float radius, float distance, LayerMask layerMask, ref RaycastHit2D[] results, Func<T, bool> conditions) where T : Component
        {
            var value = new List<T>();
            
            var size = Physics2D.CircleCastNonAlloc(from, radius, dir, results, distance, layerMask);
            
            for (int i = 0; i < size; i++)
            {
                var col = results[i];

                var t = col.collider.GetComponentInParent<T>();
                if (t == null) continue;
                if (value.Contains(t)) continue;
                if (!conditions.Invoke(t)) continue;
                value.Add(t);
            }
            
            return value;
        }
        
        //FIRST IN ALL

        /// <summary>
        /// CircleCast all and return the first target of type T
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="radius"></param>
        /// <param name="distance"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CircleCastAllAndReturnFirst<T>(Vector2 from, Vector2 dir, float radius, float distance, LayerMask layerMask, ref RaycastHit2D[] results) where T : Component
        {
            var size = Physics2D.CircleCastNonAlloc(from, radius,dir, results, distance,layerMask);
            
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
        /// CircleCast all and return the first target of type T
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="radius"></param>
        /// <param name="distance"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <param name="conditions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CircleCastAllAndReturnFirst<T>(Vector2 from, Vector2 dir, float radius, float distance, LayerMask layerMask, ref RaycastHit2D[] results, Func<T, bool> conditions) where T : Component
        {
            var size = Physics2D.CircleCastNonAlloc(from, radius,dir, results, distance,layerMask);
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
        /// CircleCast all and return the first target of type T with raycastHit2D infos
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="radius"></param>
        /// <param name="distance"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RaycastHit2D CircleCastAllAndReturnFirst<T>(Vector2 from, Vector2 dir, float radius, float distance, LayerMask layerMask, ref RaycastHit2D[] results, out T value) where T : Component
        {
            var size = Physics2D.CircleCastNonAlloc(from, radius,dir, results, distance,layerMask);
            
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
        /// CircleCast all and return the first target of type T with raycastHit2D infos
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="radius"></param>
        /// <param name="distance"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <param name="value"></param>
        /// <param name="conditions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RaycastHit2D CircleCastAllAndReturnFirst<T>(Vector2 from, Vector2 dir, float radius, float distance, LayerMask layerMask, ref RaycastHit2D[] results, out T value, Func<T, bool> conditions) where T : Component
        {
            var size = Physics2D.CircleCastNonAlloc(from, radius,dir, results, distance,layerMask);
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
        
        public static RaycastHit2D CircleCast<T>(Vector2 from, Vector2 dir, float radius, float distance, LayerMask layerMask, out T value) where T : Component
        {
            var hit = Physics2D.CircleCast(from, radius,dir, distance,layerMask);
            value = hit? hit.collider.GetComponentInParent<T>() : null;
            return hit;
        }
        
        public static RaycastHit2D CircleCast<T>(Vector2 from, Vector2 dir, float radius, float distance, LayerMask layerMask, out T value, Func<T, bool> conditions) where T : Component
        {
            var hit = Physics2D.CircleCast(from, radius,dir, distance,layerMask);
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
        
        public static RaycastHit2D CircleCast(Vector2 from, Vector2 dir, float radius, float distance, LayerMask layerMask) => Physics2D.CircleCast(from, radius,dir, distance,layerMask);
        
        public static RaycastHit2D CircleCast(Vector2 from, Vector2 dir, float radius, float distance, LayerMask layerMask, Func<RaycastHit2D, bool> conditions)
        {
            var hit = Physics2D.CircleCast(from, radius, dir, distance, layerMask);
            return conditions.Invoke(hit) ? hit : default;
        }

        
        
        //GIZMOS
        
        public static void DrawCircleCast(Vector2 from,  Vector2 dir,  float radius, float distance)
        {
            GizmosExtensions.DrawCircleCast(from,  dir * distance, radius);
        }
    }
}
