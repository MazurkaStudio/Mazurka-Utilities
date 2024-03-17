using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazurkaStudio.Utilities.Sensors
{
    public static partial class SensorUtils
    {
        //ALL
        
        /// <summary>
        /// Line cast all and return a list of all target found
        /// </summary>
        /// <param name="from"></param>
        /// <param name="target"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> LineCastAll<T>(Vector2 from, Vector2 target, LayerMask layerMask, ref RaycastHit2D[] results) where T : Component
        {
            var value = new List<T>();
            
            var size = Physics2D.LinecastNonAlloc(from, target, results,layerMask);
            
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
        /// Line cast all and return a list of all target found
        /// </summary>
        /// <param name="from"></param>
        /// <param name="target"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <param name="conditions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> LineCastAll<T>(Vector2 from, Vector2 target, LayerMask layerMask, ref RaycastHit2D[] results, Func<T, bool> conditions) where T : Component
        {
            var value = new List<T>();
            
            var size = Physics2D.LinecastNonAlloc(from, target, results,layerMask);
            
            for (var i = 0; i < size; i++)
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
        /// Raycast all along the ray and return the first target of type T
        /// </summary>
        /// <param name="from"></param>
        /// <param name="target"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LineCastAllAndReturnFirst<T>(Vector2 from, Vector2 target, LayerMask layerMask, ref RaycastHit2D[] results) where T : Component
        {
            var size = Physics2D.LinecastNonAlloc(from, target, results, layerMask);
            
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
        /// Raycast all along the ray and return the first target of type T with all conditions are true
        /// </summary>
        /// <param name="from"></param>
        /// <param name="target"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <param name="conditions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LineCastAllAndReturnFirst<T>(Vector2 from, Vector2 target, LayerMask layerMask, ref RaycastHit2D[] results, Func<T, bool> conditions) where T : Component
        {
            var size = Physics2D.LinecastNonAlloc(from, target, results, layerMask);
            var alreadyChecked = new HashSet<T>();
            
            for (var i = 0; i < size; i++)
            {
                var col = results[i];
                var result = col.collider.GetComponentInParent<T>();
                if (result == null) continue;
                if (alreadyChecked.Contains(result)) continue;
                alreadyChecked.Add(result);
                if (conditions.Invoke(result)) return result;
            }

            return null;
        }
        
        
        
        /// <summary>
        /// Raycast all along the ray and return the first target of type T with raycastHit2D infos
        /// </summary>
        /// <param name="from"></param>
        /// <param name="target"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RaycastHit2D LineCastAllAndReturnFirst<T>(Vector2 from, Vector2 target, LayerMask layerMask, ref RaycastHit2D[] results, out T value) where T : Component
        {
            var size = Physics2D.LinecastNonAlloc(from, target, results, layerMask);
            
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
        /// <param name="target"></param>
        /// <param name="layerMask"></param>
        /// <param name="results"></param>
        /// <param name="value"></param>
        /// <param name="conditions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RaycastHit2D LineCastAllAndReturnFirst<T>(Vector2 from, Vector2 target, LayerMask layerMask, ref RaycastHit2D[] results, out T value, Func<T, bool> conditions) where T : Component
        {
            var size = Physics2D.LinecastNonAlloc(from, target, results, layerMask);
            var alreadyChecked = new HashSet<T>();
         
            for (var i = 0; i < size; i++)
            {
                var col = results[i];
                var result = col.collider.GetComponentInParent<T>();
                
                if (result == null) continue;
                
                if (alreadyChecked.Contains(result)) continue;
                alreadyChecked.Add(result);
                
                if (conditions.Invoke(result))
                {
                    value = result;
                    return col;
                }
            }

            value = null;
            return default;
        }

        
        //SIMPLE
        
        /// <summary>
        /// Simple line cast + get component in parent
        /// </summary>
        /// <param name="from"></param>
        /// <param name="target"></param>
        /// <param name="layerMask"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RaycastHit2D LineCast<T>(Vector2 from, Vector2 target, LayerMask layerMask, out T value) where T : Component
        {
            var hit = Physics2D.Linecast(from, target,layerMask);
            value = hit? hit.collider.GetComponentInParent<T>() : null;
            return hit;
        }
        
        /// <summary>
        /// Simple line cast + get component in parent
        /// </summary>
        /// <param name="from"></param>
        /// <param name="target"></param>
        /// <param name="layerMask"></param>
        /// <param name="value"></param>
        /// <param name="conditions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RaycastHit2D LineCast<T>(Vector2 from, Vector2 target, LayerMask layerMask, out T value, Func<T, bool> conditions) where T : Component
        {
            var hit = Physics2D.Linecast(from, target,layerMask);
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
        /// Simple linecast
        /// </summary>
        /// <param name="from"></param>
        /// <param name="target"></param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public static RaycastHit2D LineCast(Vector2 from, Vector2 target, LayerMask layerMask) => Physics2D.Linecast(from, target,layerMask);
        
        public static RaycastHit2D LineCast(Vector2 from, Vector2 target, LayerMask layerMask, Func<RaycastHit2D, bool> conditions)
        {
            var hit = Physics2D.Linecast(from, target, layerMask);
            return conditions.Invoke(hit) ? hit : default;
        }


        public static bool Check<T>(T value)
        {
          
           
            return true;
        }
        
        public static void DrawLineCast(Vector2 from,  Vector2 target)
        {
            Gizmos.DrawLine(from, target);
        }
    }
}