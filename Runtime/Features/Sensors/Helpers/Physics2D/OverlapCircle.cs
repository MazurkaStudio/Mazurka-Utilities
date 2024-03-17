using System.Collections.Generic;
using UnityEngine;

namespace TheMazurkaStudio.Utilities.Sensors
{
    public static partial class SensorUtils 
    {
        public static List<T> OverlapCircleAll<T>(Vector2 from, float radius, LayerMask layerMask, ref Collider2D[] results)
        { 
            var value = new List<T>();
            
            var size = Physics2D.OverlapCircleNonAlloc(from, radius, results,layerMask);
            
            for (int i = 0; i < size; i++)
            {
                var col = results[i];

                var t = col.GetComponentInParent<T>();
                
                if (t == null) continue;
                
                if(!value.Contains(t)) value.Add(t);
            }
            
            return value;
        }
        public static RaycastHit2D OverlapCircle<T>(Vector3 from, float radius, LayerMask layerMask, out T value) where T : Component
        {
            var col = Physics2D.OverlapCircle(from, radius, layerMask);
            if (col != null)
            {
                var targetPos = col.ClosestPoint(from);
                var delta = targetPos - (Vector2)from;
                return RayCast(from, delta.normalized, delta.magnitude + 0.1f, layerMask, out value);
            }

            value = null;
            return default;
        }
        
        public static RaycastHit2D OverlapCircle(Vector3 from, float radius, LayerMask layerMask)
        {
            var col = Physics2D.OverlapCircle(from, radius, layerMask);
            if (col != null)
            {
                var targetPos = col.ClosestPoint(from);
                var delta = targetPos - (Vector2)from;
                return RayCast(from, delta.normalized, delta.magnitude + 0.1f, layerMask);
            }

            return default;
        }

        public static void DrawOverlapCircle(Vector3 from, float radius)
        {
            Gizmos.DrawWireSphere(from, radius);
        }
    }
}