using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheMazurkaStudio.Utilities.Sensors
{
    public static partial class SensorUtils
    {
        public static Vector2[] BurstDirections(Vector2 lookAtDir, int rayCount, float angle, float offsetDeg)
        {
            if (rayCount <= 1)
            {
                var a = Mathf.Atan2(lookAtDir.y, lookAtDir.x) + Mathf.Deg2Rad * offsetDeg;
                var d = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
                return new[] { d };
            }
            
            var angleIncrement = angle / (rayCount - 1); // Divided by (rayCount - 1) to include the angle range properly.
            var dir = new Vector2[rayCount];
    
            offsetDeg *= Mathf.Deg2Rad;
            var lookAtAngle = Mathf.Atan2(lookAtDir.y, lookAtDir.x); // Calculate angle using Mathf.Atan2
    
            for (int i = 0; i < rayCount; i++) 
            {
                var currentAngle = lookAtAngle + Mathf.Deg2Rad * (-angle / 2 + i * angleIncrement) + offsetDeg; // Correct angle calculation
                dir[i] = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));
            }
    
            return dir;
        }
        
        //ALL
        public static List<T> BurstCastAll<T>(Vector2 from, float distance, LayerMask layerMask, int burstCount, Vector2 lookAtDir, float angleDeg, float offsetDeg, float distanceOffset, ref RaycastHit2D[] results) where T : Component
        {
            var value = new List<T>();
            
            var directions = BurstDirections(lookAtDir, burstCount, angleDeg, offsetDeg);
            
            for (int j = 0; j < burstCount; j++)
            {
                var dir = directions[j];
                var fromPos = from + dir * distanceOffset;
                value.AddRange(RayCastAll<T>(fromPos, dir, distance, layerMask, ref results).Except(value));
            }

            return value;
        }
    
        public static List<T> BurstCastAll<T>(Vector2 from, float distance, LayerMask layerMask, int burstCount, Vector2 lookAtDir, float angleDeg, float offsetDeg, float distanceOffset, ref RaycastHit2D[] results, Func<T, bool> conditions) where T : Component
        {
            var value = new List<T>();
            
            var directions = BurstDirections(lookAtDir, burstCount, angleDeg, offsetDeg);
            
            for (int j = 0; j < burstCount; j++)
            {
                var dir = directions[j];
                var fromPos = from + dir * distanceOffset;
                value.AddRange(RayCastAll(fromPos, dir, distance, layerMask, ref results, conditions).Except(value));
            }

            return value;
        }
        
        //FIRST IN ALL

        public static T BurstCastAllAndReturnFirst<T>(Vector2 from, float distance, LayerMask layerMask, int burstCount, Vector2 lookAtDir, float angleDeg, float offsetDeg, float distanceOffset, ref RaycastHit2D[] results) where T : Component
        {
            var directions = BurstDirections(lookAtDir, burstCount, angleDeg, offsetDeg);
            
            for (int j = 0; j < burstCount; j++)
            {
                var dir = directions[j];
                var fromPos = from + dir * distanceOffset;
                var target = RayCastAllAndReturnFirst<T>(fromPos, dir, distance, layerMask, ref results);
                if (target != null) return target;
            }

            return null;
        }
        
        public static T BurstCastAllAndReturnFirst<T>(Vector2 from, float distance, LayerMask layerMask, int burstCount, Vector2 lookAtDir, float angleDeg, float offsetDeg, float distanceOffset, ref RaycastHit2D[] results, Func<T, bool> conditions) where T : Component
        {
            var directions = BurstDirections(lookAtDir, burstCount, angleDeg, offsetDeg);

            for (int j = 0; j < burstCount; j++)
            {
                var dir = directions[j];
                var fromPos = from + dir * distanceOffset;
                var target = RayCastAllAndReturnFirst(fromPos, dir, distance, layerMask, ref results, conditions);
                
                if (target != null) return target;
            }

            return null;
        }
        
        
        public static RaycastHit2D BurstCastAllAndReturnFirst<T>(Vector2 from, float distance, LayerMask layerMask, int burstCount, Vector2 lookAtDir, float angleDeg, float offsetDeg, float distanceOffset, out T value, ref RaycastHit2D[] results) where T : Component
        {
            var directions = BurstDirections(lookAtDir, burstCount, angleDeg, offsetDeg);
            
            for (int j = 0; j < burstCount; j++)
            {
                var dir = directions[j];
                var fromPos = from + dir * distanceOffset;
                var hit = RayCastAllAndReturnFirst(fromPos, dir, distance, layerMask, ref results, out T target);
                
                if (target != null)
                {
                    value = target;
                    return hit;
                }
            }

            value = null;
            return default;
        }
       
        public static RaycastHit2D BurstCastAllAndReturnFirst<T>(Vector2 from, float distance, LayerMask layerMask, int burstCount, Vector2 lookAtDir, float angleDeg, float offsetDeg, float distanceOffset, out T value, ref RaycastHit2D[] results, Func<T, bool> conditions) where T : Component
        {
            var directions = BurstDirections(lookAtDir, burstCount, angleDeg, offsetDeg);
            
            for (int j = 0; j < burstCount; j++)
            {
                var dir = directions[j];
                var fromPos = from + dir * distanceOffset;
                var hit = RayCastAllAndReturnFirst(fromPos, dir, distance, layerMask, ref results, out var target, conditions);
                
                if (target != null)
                {
                    value = target;
                    return hit;
                }
            }
            
            value = null;
            return default;
        }
        
        
        
        //SIMPLE
        
        public static RaycastHit2D BurstCast<T>(Vector2 from, float distance, LayerMask layerMask, int burstCount, Vector2 lookAtDir, float angleDeg, float offsetDeg, float distanceOffset, out T value) where T : Component
        {
            var directions = BurstDirections(lookAtDir, burstCount, angleDeg, offsetDeg);
            
            for (int j = 0; j < burstCount; j++)
            {
                var dir = directions[j];
                var fromPos = from + dir * distanceOffset;
                var hit = RayCast(fromPos, dir, distance, layerMask, out value);
                if (hit && value != null) return hit;
            }

            value = null;
            return default;
        }
        public static RaycastHit2D BurstCast<T>(Vector2 from, float distance, LayerMask layerMask, int burstCount, Vector2 lookAtDir, float angleDeg, float offsetDeg, float distanceOffset, out T value, Func<T, bool> conditions) where T : Component
        {
            var directions = BurstDirections(lookAtDir, burstCount, angleDeg, offsetDeg);
            var alreadyChecked = new HashSet<Collider2D>();
            var alreadyCheckedTargets = new HashSet<T>();
            
            for (int j = 0; j < burstCount; j++)
            {
                var dir = directions[j];
                var fromPos = from + dir * distanceOffset;
                var hit = Physics2D.Raycast(fromPos, dir, distance, layerMask);
                
                if(!hit) continue; 
                
                if (alreadyChecked.Contains(hit.collider)) continue;
                alreadyChecked.Add(hit.collider);

                var target = hit.collider.GetComponentInParent<T>();
                
                if (target == null) continue;
                               
                if (alreadyCheckedTargets.Contains(target)) continue;
                alreadyCheckedTargets.Add(target);

                if (!conditions.Invoke(target)) continue;
                
                value = target;
                return hit;
            }

            value = null;
            return default;
        }
        public static RaycastHit2D BurstCast(Vector2 from, float distance, LayerMask layerMask, int burstCount, Vector2 lookAtDir, float angleDeg, float offsetDeg, float distanceOffset)
        {
            var directions = BurstDirections(lookAtDir, burstCount, angleDeg, offsetDeg);
            
            for (int j = 0; j < burstCount; j++)
            {
                var dir = directions[j];
                var fromPos = from + dir * distanceOffset;
                var hit = RayCast(fromPos, dir, distance, layerMask);
                if (hit) return hit;
            }
            
            return default;
        }
        
        //GIZMOS
        public static void DrawBurstCast(Vector2 from, float distance, int burstCount, Vector2 lookAtDir, float angleDeg, float offsetDeg, float distanceOffset)
        {
            var directions = BurstDirections(lookAtDir, burstCount, angleDeg, offsetDeg);
            
            for (int j = 0; j < burstCount; j++)
            {
                var dir = directions[j];
                var pos = from + dir * distanceOffset;
                DrawRayCast(pos, dir, distance);
            }
        }
    }
}
