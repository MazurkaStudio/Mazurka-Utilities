using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheMazurkaStudio.Utilities.Sensors
{
    public static partial class SensorUtils
    {
        //ALL
        public static List<T> LadderCastAll<T>(Vector2 from, Vector2 dir, float distance, LayerMask layers, float stepSize, Vector2 stepDir, int stepCount, ref RaycastHit2D[] results) where T : Component
        {
            var value = new List<T>();
            
            var pos = from;
            
            for (int i = 0; i < stepCount; i++)
            {
                value.AddRange( RayCastAll<T>(pos, dir, distance, layers, ref results).Except(value));
                pos += stepDir * stepSize;
            }
            
            return value;
        }
        
        public static List<T> LadderCastAll<T>(Vector2 from, Vector2 dir, float distance, LayerMask layers, float stepSize, Vector2 stepDir, int stepCount, ref RaycastHit2D[] results, Func<T, bool> conditions) where T : Component
        {
            var value = new List<T>();
            
            var pos = from;
            
            for (int i = 0; i < stepCount; i++)
            {
                value.AddRange( RayCastAll(pos, dir, distance, layers, ref results, conditions).Except(value));
                pos += stepDir * stepSize;
            }
            
            return value;
        }
        
        //FIRST IN ALL
        
        public static T LadderCastAllAndReturnFirst<T>(Vector2 from, Vector2 dir, float distance, LayerMask layers, float stepSize, Vector2 stepDir, int stepCount, ref RaycastHit2D[] results) where T : Component
        {
            var pos = from;
            
            for (int i = 0; i < stepCount; i++)
            {
                var hit = RayCastAllAndReturnFirst<T>(pos, dir, distance, layers, ref results);
                
                if (hit != null)
                {
                    return hit;
                }
                
                pos += stepDir * stepSize;
            }

            return null;
        }
        public static T LadderCastAllAndReturnFirst<T>(Vector2 from, Vector2 dir, float distance, LayerMask layers, float stepSize, Vector2 stepDir, int stepCount, ref RaycastHit2D[] results, Func<T, bool> conditions) where T : Component
        {
            var pos = from;
            var alreadyChecked = new HashSet<T>();
            
            for (int i = 0; i < stepCount; i++)
            {
                var hit = RayCastAllAndReturnFirst<T>(pos, dir, distance, layers, ref results);
                
                if (hit != null)
                {
                    if (alreadyChecked.Contains(hit)) continue;
                    alreadyChecked.Add(hit);
                    
                    if(conditions.Invoke(hit))
                    {
                        return hit;
                    }
                }
                
                pos += stepDir * stepSize;
            }

            return null;
        }
        
        public static RaycastHit2D LadderCastAllAndReturnFirst<T>(Vector2 from, Vector2 dir, float distance, LayerMask layers, float stepSize, Vector2 stepDir, int stepCount, ref RaycastHit2D[] results, out T value) where T : Component
        {
            var pos = from;
            
            for (int i = 0; i < stepCount; i++)
            {
                var hit = RayCastAllAndReturnFirst(pos, dir, distance, layers, ref results, out T target);
                
                if (target != null)
                {
                    value = target;
                    return hit;
                }
                
                pos += stepDir * stepSize;
            }

            value = null;
            return default;
        }
        public static RaycastHit2D LadderCastAllAndReturnFirst<T>(Vector2 from, Vector2 dir, float distance, LayerMask layers, float stepSize, Vector2 stepDir, int stepCount, ref RaycastHit2D[] results, out T value, Func<T, bool> conditions) where T : Component
        {
            var pos = from;
            var alreadyChecked = new HashSet<T>();
            
            for (int i = 0; i < stepCount; i++)
            {
                var hit = RayCastAllAndReturnFirst(pos, dir, distance, layers, ref results, out T target);
                
                if (target != null)
                {
                    if (alreadyChecked.Contains(target)) continue;
                    alreadyChecked.Add(target);
                    
                    if (conditions.Invoke(target))
                    {
                        value = target;
                        return hit;
                    }
                }
                
                pos += stepDir * stepSize;
            }

            value = null;
            return default;
        }
        
        
        //SIMPLE
        
        /// <summary>
        /// Return the first ray in ladder that hit something of type <typeparam name="T"></typeparam>
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="layers"></param>
        /// <param name="stepSize"></param>
        /// <param name="stepDir"></param>
        /// <param name="stepCount"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RaycastHit2D LadderCast<T>(Vector2 from, Vector2 dir, float distance, LayerMask layers, float stepSize, Vector2 stepDir, int stepCount, out T value) where T : Component
        {
            var pos = from;
            for (int i = 0; i < stepCount; i++)
            {
                var hit = RayCast(pos, dir, distance, layers, out value);
                if(hit && value != null) return hit;
                pos += stepDir * stepSize;
            }

            value = null;
            return default;
        }
        
        /// <summary>
        /// Return the first ray in ladder that hit something of type <typeparam name="T"></typeparam> with all conditions valids
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="layers"></param>
        /// <param name="stepSize"></param>
        /// <param name="stepDir"></param>
        /// <param name="stepCount"></param>
        /// <param name="value"></param>
        /// <param name="conditions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static RaycastHit2D LadderCast<T>(Vector2 from, Vector2 dir, float distance, LayerMask layers, float stepSize, Vector2 stepDir, int stepCount, out T value, Func<T, bool> conditions) where T : Component
        {
            var pos = from;
            
            var alreadyChecked = new HashSet<T>();

            for (int i = 0; i < stepCount; i++)
            {
                var hit = RayCast(pos, dir, distance, layers, out T target);
                
                if (target != null)
                {
                    if (alreadyChecked.Contains(target)) continue;
                    alreadyChecked.Add(target);
                    
                    if (conditions.Invoke(target))
                    {
                        value = target;
                        return hit;
                    }
                }
                
                pos += stepDir * stepSize;
            }

            value = null;
            return default;
        }
        
        /// <summary>
        /// Return the first ray in ladder that hit something
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="layers"></param>
        /// <param name="stepSize"></param>
        /// <param name="stepDir"></param>
        /// <param name="stepCount"></param>
        /// <returns></returns>
        public static RaycastHit2D LadderCast(Vector2 from, Vector2 dir, float distance, LayerMask layers, float stepSize, Vector2 stepDir, int stepCount) 
        {
            var pos = from;
            for (int i = 0; i < stepCount; i++)
            {
                var hit = Physics2D.Raycast(pos, dir, distance, layers);
                if(hit) return hit;
                
                pos += stepDir * stepSize;
            }
            return default;
        }
        
        /// <summary>
        /// Return the first ray in ladder that hit something with all conditions valids
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="layers"></param>
        /// <param name="stepSize"></param>
        /// <param name="stepDir"></param>
        /// <param name="stepCount"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static RaycastHit2D LadderCast(Vector2 from, Vector2 dir, float distance, LayerMask layers, float stepSize, Vector2 stepDir, int stepCount, Func<RaycastHit2D, bool> conditions) 
        {
            var pos = from;
            
            for (int i = 0; i < stepCount; i++)
            {
                var hit = Physics2D.Raycast(pos, dir, distance, layers);
                
                if (hit)
                {
                    if(conditions.Invoke(hit)) return hit;
                }
                
                pos += stepDir * stepSize;
            }
            return default;
        }
        
        //GIZMOS
        public static void DrawLadderCast(Vector2 from, Vector2 dir, float distance, float stepSize, Vector2 stepDir, int stepCount)
        {
            var pos = from;
            for (int i = 0; i < stepCount; i++)
            {
                Gizmos.DrawLine(pos, pos + dir * distance);
                pos += stepDir * stepSize;
            }
        }

        /// <summary>
        /// Return one raycastHit2D result per ray
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dir"></param>
        /// <param name="distance"></param>
        /// <param name="layers"></param>
        /// <param name="stepSize"></param>
        /// <param name="stepDir"></param>
        /// <param name="hitResults"></param>
        /// <returns></returns>
        public static void LadderCastAny(Vector3 from, Vector3 dir, float distance, LayerMask layers, float stepSize, Vector3 stepDir, ref RaycastHit2D[] hitResults)
        {
            var pos = from;
            for (int i = 0; i < hitResults.Length; i++)
            {
                var hit = RayCast(pos, dir, distance, layers);
                hitResults[i] = hit;
                pos += stepDir * stepSize;
            }
        }
    }
}