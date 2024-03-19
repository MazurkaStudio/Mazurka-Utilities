using System;
using System.Collections.Generic;
using Stump.TheMazurkaStudio.Utilities;
using UnityEngine;

namespace TheMazurkaStudio.Utilities.Sensors
{
    /// <summary>
    /// Cast with physics to find actors of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Sensor<T> where T : Component
    {
        public Sensor()
        {
            hitResults = new RaycastHit2D[maxCastCountPerRay];
            colResults = new Collider2D[maxCastCountPerRay];
        }

        [SerializeField] protected SensorProfile profile;
        [SerializeField] protected Color debugColor = Color.blue;
        [SerializeField] private Transform transform;
        [SerializeField] private Transform transformTarget;
        [SerializeField] private int maxCastCountPerRay = 8;
        [SerializeField] private int maxCastCount = 16;
        [SerializeField] private LayerMask seekLayer;

        [SerializeField] private SensorParameters parameters;
        
        //DEBUG ONLY
        private bool haveCast;
        private bool haveHit;
        private float lastTimeHit;
        
        private Collider2D[] colResults;
        private RaycastHit2D[] hitResults;
        
        public LayerMask SensorLayers => HaveProfile ? profile.seekLayer : seekLayer;
        public bool HaveProfile => profile != null;

        private Vector2 CastPosition => transform.position + transform.TransformDirection(parameters.castOffset);

        /// <summary>
        /// Return the first item of type T found in the sensor if exist and RaycastHit2D value to reach it
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public RaycastHit2D SeekFirst(out T value)
        {
            haveCast = true;
            haveHit = false;
            value = null;

            var cast = parameters.seekMethode switch
            {
                SeekMethode.Raycast => SensorUtils.RayCast(CastPosition, transform.right, parameters.castDistance, SensorLayers, out value),
                SeekMethode.LineCast => SensorUtils.LineCast(CastPosition, transformTarget.position, SensorLayers, out value),
                SeekMethode.CircleCast => SensorUtils.CircleCast(CastPosition, transform.right, parameters.radius, parameters.castDistance, SensorLayers, out value),
                SeekMethode.BoxCast => SensorUtils.BoxCast(CastPosition, transform.right, parameters.size, parameters.angle, parameters.castDistance, SensorLayers, out value),
                SeekMethode.OverlapSphere => SensorUtils.OverlapCircle(CastPosition, parameters.radius, SensorLayers, out value),
                SeekMethode.Burst => SensorUtils.BurstCast(CastPosition, parameters.castDistance, SensorLayers, parameters.stepCount, transform.right, parameters.angle, parameters.offset, parameters.distanceOffset, out value),
                SeekMethode.Ladder => SensorUtils.LadderCast(CastPosition, transform.right, parameters.castDistance, SensorLayers, parameters.stepSize, parameters.angle.ToVector(), parameters.stepCount, out value),
                _ => default
            };

            if (!cast) return default;

            haveHit = true;
            lastTimeHit = Time.unscaledTime;
            return cast;
        }
        
        public RaycastHit2D SeekFirst(out T value, Func<T, bool> conditions)
        {
            haveCast = true;
            haveHit = false;
            value = null;

            var cast = parameters.seekMethode switch
            {
                SeekMethode.Raycast => SensorUtils.RayCast(CastPosition, transform.right, parameters.castDistance, SensorLayers, out value, conditions),
                SeekMethode.LineCast => SensorUtils.LineCast(CastPosition, transformTarget.position, SensorLayers, out value, conditions),
                SeekMethode.CircleCast => SensorUtils.CircleCast(CastPosition, transform.right, parameters.radius, parameters.castDistance, SensorLayers, out value, conditions),
                SeekMethode.BoxCast => SensorUtils.BoxCast(CastPosition, transform.right, parameters.size, parameters.angle, parameters.castDistance, SensorLayers, out value, conditions),
                SeekMethode.Burst => SensorUtils.BurstCast(CastPosition, parameters.castDistance, SensorLayers, parameters.stepCount, transform.right, parameters.angle, parameters.offset, parameters.distanceOffset, out value, conditions),
                SeekMethode.Ladder => SensorUtils.LadderCast(CastPosition, transform.right, parameters.castDistance, SensorLayers, parameters.stepSize, parameters.angle.ToVector(), parameters.stepCount, out value, conditions),
                _ => default
            };

            if (!cast) return default;

            haveHit = true;
            lastTimeHit = Time.unscaledTime;
            return cast;
        }
        
        /// <summary>
        /// Return the first item of type T found in the sensor if exist and RaycastHit2D value to reach it
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public RaycastHit2D SeekAllAndReturnFirst(out T value)
        {
            haveCast = true;
            haveHit = false;
            value = null;

            var cast = parameters.seekMethode switch
            {
                SeekMethode.Raycast => SensorUtils.RayCastAllAndReturnFirst(CastPosition, transform.right, parameters.castDistance, SensorLayers, ref hitResults, out value),
                SeekMethode.LineCast => SensorUtils.LineCastAllAndReturnFirst(CastPosition, transformTarget.position, SensorLayers, ref hitResults, out value),
                SeekMethode.CircleCast => SensorUtils.CircleCastAllAndReturnFirst(CastPosition, transform.right, parameters.radius, parameters.castDistance, SensorLayers, ref hitResults, out value),
                SeekMethode.BoxCast => SensorUtils.BoxCastAllAndReturnFirst(CastPosition, transform.right, parameters.size, parameters.angle, parameters.castDistance, SensorLayers, ref hitResults, out value),
                SeekMethode.OverlapSphere => SensorUtils.OverlapCircle(CastPosition, parameters.radius, SensorLayers, out value),
                SeekMethode.Burst => SensorUtils.BurstCastAllAndReturnFirst(CastPosition, parameters.castDistance, SensorLayers, parameters.stepCount, transform.right, parameters.angle, parameters.offset, parameters.distanceOffset, out value, ref hitResults),
                SeekMethode.Ladder => SensorUtils.LadderCastAllAndReturnFirst(CastPosition, transform.right, parameters.castDistance, SensorLayers, parameters.stepSize, parameters.angle.ToVector(), parameters.stepCount, ref hitResults, out value),
                _ => default
            };

            if (!cast) return default;

            haveHit = true;
            lastTimeHit = Time.unscaledTime;
            return cast;
        }
        
        /// <summary>
        /// Return the first item of type T found in the sensor if exist and RaycastHit2D value to reach it
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RaycastHit2D SeekAllAndReturnFirst(out T value, Func<T, bool> conditions)
        {
            haveCast = true;
            haveHit = false;
            value = null;

            var cast = parameters.seekMethode switch
            {
                SeekMethode.Raycast => SensorUtils.RayCastAllAndReturnFirst(CastPosition, transform.right, parameters.castDistance, SensorLayers, ref hitResults, out value, conditions),
                SeekMethode.LineCast => SensorUtils.LineCastAllAndReturnFirst(CastPosition, transformTarget.position, SensorLayers, ref hitResults, out value, conditions),
                SeekMethode.CircleCast => SensorUtils.CircleCastAllAndReturnFirst(CastPosition, transform.right, parameters.radius, parameters.castDistance, SensorLayers, ref hitResults, out value, conditions),
                SeekMethode.BoxCast => SensorUtils.BoxCastAllAndReturnFirst(CastPosition, transform.right, parameters.size, parameters.angle, parameters.castDistance, SensorLayers, ref hitResults, out value, conditions),
                SeekMethode.Burst => SensorUtils.BurstCastAllAndReturnFirst(CastPosition, parameters.castDistance, SensorLayers, parameters.stepCount, transform.right, parameters.angle, parameters.offset, parameters.distanceOffset, out value, ref hitResults, conditions),
                SeekMethode.Ladder => SensorUtils.LadderCastAllAndReturnFirst(CastPosition, transform.right, parameters.castDistance, SensorLayers, parameters.stepSize, parameters.angle.ToVector(), parameters.stepCount, ref hitResults, out value, conditions),
                _ => default
            };

            if (!cast) return default;

            haveHit = true;
            lastTimeHit = Time.unscaledTime;
            return cast;
        }

        
        /// <summary>
        /// Return all actor of type T in the sensor without duplicates
        /// </summary>
        /// <returns></returns>
        public List<T> SeekAll()
        {
            haveCast = true;
            haveHit = false;
            
            var value = parameters.seekMethode switch
            {
                SeekMethode.Raycast => SensorUtils.RayCastAll<T>(CastPosition, transform.right, parameters.castDistance, SensorLayers, ref hitResults),
                SeekMethode.LineCast => SensorUtils.LineCastAll<T>(CastPosition, transformTarget.position, SensorLayers, ref hitResults),
                SeekMethode.CircleCast => SensorUtils.CircleCastAll<T>(CastPosition, transform.right, parameters.radius, parameters.castDistance, SensorLayers, ref hitResults),
                SeekMethode.BoxCast => SensorUtils.BoxCastAll<T>(CastPosition, transform.right, parameters.size,parameters.angle, parameters.castDistance, SensorLayers, ref hitResults),
                SeekMethode.OverlapSphere => SensorUtils.OverlapCircleAll<T>(CastPosition, parameters.radius, SensorLayers, ref colResults),
                SeekMethode.Burst => SensorUtils.BurstCastAll<T>(CastPosition, parameters.castDistance, SensorLayers, parameters.stepCount, transform.right, parameters.angle, parameters.offset, parameters.distanceOffset, ref hitResults),
                SeekMethode.Ladder => SensorUtils.LadderCastAll<T>(CastPosition, transform.right, parameters.castDistance, SensorLayers, parameters.stepSize, parameters.angle.ToVector(), parameters.stepCount, ref hitResults),
                SeekMethode.None => null,
                _ => null
            };

            if (value == null) return null;
            
            if (value.Count > 0)
            {
                haveHit = true;
                lastTimeHit = Time.unscaledTime;
                return value;
            }

            return null;
        }
        
        /// <summary>
        /// Return all actor of type T in the sensor without duplicates
        /// </summary>
        /// <returns></returns>
        public List<T> SeekAll(Func<T, bool> conditions)
        {
            haveCast = true;
            haveHit = false;
            
            var value = parameters.seekMethode switch
            {
                SeekMethode.Raycast => SensorUtils.RayCastAll<T>(CastPosition, transform.right, parameters.castDistance, SensorLayers, ref hitResults, conditions),
                SeekMethode.LineCast => SensorUtils.LineCastAll<T>(CastPosition, transformTarget.position, SensorLayers, ref hitResults, conditions),
                SeekMethode.CircleCast => SensorUtils.CircleCastAll<T>(CastPosition, transform.right, parameters.radius, parameters.castDistance, SensorLayers, ref hitResults, conditions),
                SeekMethode.BoxCast => SensorUtils.BoxCastAll<T>(CastPosition, transform.right, parameters.size, parameters.angle, parameters.castDistance, SensorLayers, ref hitResults, conditions),
                SeekMethode.OverlapSphere => SensorUtils.OverlapCircleAll<T>(CastPosition, parameters.radius, SensorLayers, ref colResults),
                SeekMethode.Burst => SensorUtils.BurstCastAll<T>(CastPosition, parameters.castDistance, SensorLayers, parameters.stepCount, transform.right, parameters.angle, parameters.offset, parameters.distanceOffset, ref hitResults, conditions),
                SeekMethode.Ladder => SensorUtils.LadderCastAll<T>(CastPosition, transform.right, parameters.castDistance, SensorLayers, parameters.stepSize, parameters.angle.ToVector(), parameters.stepCount, ref hitResults, conditions),
                SeekMethode.None => null,
                _ => null
            };

            if (value == null) return null;
            
            if (value.Count > 0)
            {
                haveHit = true;
                lastTimeHit = Time.unscaledTime;
                return value;
            }

            return null;
        }

        public void ApplyParameters(SensorParameters param) => param.ApplyParameters(ref parameters);
        
        public void DrawGizmos()
        {
            if (transform == null) return;

            var color = HaveProfile ? profile.color : debugColor;
            
            if (Application.isPlaying)
            {
                color = haveHit ? Color.green : Color.red;
                
                if (!haveCast)
                {
                    color = Color.black;
                    color.a = 0.5f;
                }
            }
            
            Gizmos.color = color;
            
            switch (parameters.seekMethode)
            {
                case SeekMethode.Raycast: SensorUtils.DrawRayCast(CastPosition, transform.right, parameters.castDistance); break;
                case SeekMethode.LineCast: if(transformTarget != null) SensorUtils.DrawLineCast(CastPosition, transformTarget.position); break;
                case SeekMethode.CircleCast: SensorUtils.DrawCircleCast(CastPosition, transform.right, parameters.radius, parameters.castDistance); break;
                case SeekMethode.BoxCast: SensorUtils.DrawBoxCast(CastPosition, parameters.size,transform.right, parameters.angle, parameters.castDistance); break;
                case SeekMethode.OverlapSphere: SensorUtils.DrawOverlapCircle(CastPosition, parameters.radius); break;
                case SeekMethode.Burst: SensorUtils.DrawBurstCast(CastPosition, parameters.castDistance, parameters.stepCount,transform.right, parameters.angle, parameters.offset, parameters.distanceOffset); break;
                case SeekMethode.Ladder: SensorUtils.DrawLadderCast(CastPosition, transform.right, parameters.castDistance, parameters.stepSize, parameters.angle.ToVector(), parameters.stepCount); break;
            }
            
            haveCast = false;

            if (haveHit &&Time.unscaledTime - lastTimeHit > 0.5f)
            {
                haveHit = false;
            }
        }
    }
    
    
    
    
    /// <summary>
    /// Cast with physics to find obstruction point
    /// </summary>
    [Serializable]
    public class Sensor
    {
        public Sensor()
        {
            
        }
        
        [SerializeField] protected SensorProfile profile;
        [SerializeField] protected Color debugColor = Color.black;
        [SerializeField] private Transform transform;
        [SerializeField] private Transform transformTarget;
        [SerializeField] private int maxCastCountPerRay = 8;
        [SerializeField] private int maxCastCount = 16;
        [SerializeField] private LayerMask seekLayer;
        [SerializeField] private SensorParameters parameters;

        public LayerMask SensorLayers => HaveProfile ? profile.seekLayer : seekLayer;
        public bool HaveProfile => profile != null;

        private Vector2 CastPosition => transform.position + transform.TransformDirection(parameters.castOffset);


        private bool haveCast;
        private bool haveHit;
        private float lastTimeHit;

        /// <summary>
        /// Return the first ray that hit something (can be negative)
        /// </summary>
        /// <returns></returns>
        public RaycastHit2D SeekFirst()
        {
            haveCast = true;
            haveHit = false;
            
            var cast = parameters.seekMethode switch
            {
                SeekMethode.Raycast => SensorUtils.RayCast(CastPosition, transform.right, parameters.castDistance, SensorLayers),
                SeekMethode.LineCast => SensorUtils.LineCast(CastPosition, transformTarget.position, SensorLayers),
                SeekMethode.CircleCast => SensorUtils.CircleCast(CastPosition, transform.right, parameters.radius, parameters.castDistance, SensorLayers),
                SeekMethode.BoxCast => SensorUtils.BoxCast(CastPosition, transform.right, parameters.size, parameters.angle, parameters.castDistance, SensorLayers),
                SeekMethode.OverlapSphere => SensorUtils.OverlapCircle(CastPosition, parameters.radius, SensorLayers),
                SeekMethode.Burst => SensorUtils.BurstCast(CastPosition, parameters.castDistance, SensorLayers, parameters.stepCount,transform.right, parameters.angle, parameters.offset, parameters.distanceOffset),
                SeekMethode.Ladder => SensorUtils.LadderCast(CastPosition,transform.right, parameters.castDistance, SensorLayers, parameters.stepSize,parameters.angle.ToVector(), parameters.stepCount),
                _ => default
            };
            
            if (!cast) return default;

            haveHit = true;
            lastTimeHit = Time.unscaledTime;
            return cast;
        }
        
        public void ApplyParameters(SensorParameters param) => param.ApplyParameters(ref parameters);
        public void DrawGizmos()
        {
            if (transform == null) return;
            
            var color = HaveProfile ? profile.color : debugColor;
            
            if (Application.isPlaying)
            {
                color = haveHit ? Color.green : Color.red;
                
                if (!haveCast)
                {
                    color = Color.black;
                    color.a = 0.5f;
                }
            }

            Gizmos.color = color;
            
            switch (parameters.seekMethode)
            {
                case SeekMethode.Raycast: SensorUtils.DrawRayCast(transform.position, transform.right, parameters.castDistance); break;
                case SeekMethode.LineCast: if(transformTarget != null) SensorUtils.DrawLineCast(transform.position, transformTarget.position); break;
                case SeekMethode.CircleCast: SensorUtils.DrawCircleCast(transform.position, transform.right, parameters.radius, parameters.castDistance); break;
                case SeekMethode.BoxCast: SensorUtils.DrawBoxCast(transform.position, parameters.size,transform.right, parameters.angle, parameters.castDistance); break;
                case SeekMethode.OverlapSphere: SensorUtils.DrawOverlapCircle(transform.position, parameters.radius); break;
                case SeekMethode.Burst: SensorUtils.DrawBurstCast(transform.position, parameters.castDistance, parameters.stepCount,transform.right, parameters.angle, parameters.offset, parameters.distanceOffset); break;
                case SeekMethode.Ladder: SensorUtils.DrawLadderCast(transform.position, transform.right, parameters.castDistance, parameters.stepSize, parameters.angle.ToVector(), parameters.stepCount); break;
            }

            haveCast = false;
            haveHit = false;
        }
    }

    [Serializable]
    public struct SensorParameters
    {
        public SeekMethode seekMethode;
        public Vector2 castOffset;
        public float castDistance;
        public float radius;
        public int stepCount;
        public float angle;
        public float offset;
        public float stepSize;
        public float distanceOffset;
        public Vector2 size;

        public void ApplyParameters(ref SensorParameters param)
        {
            param.castOffset = castOffset;
            param.seekMethode = seekMethode;
            param.castDistance = castDistance;
            param.radius = radius;
            param.stepCount = stepCount;
            param.angle = angle;
            param.offset = offset;
            param.stepSize = stepSize;
            param.distanceOffset = distanceOffset;
            param.size = size;
        }
        
        public static SensorParameters Default => new SensorParameters()
        {
            seekMethode = SeekMethode.Raycast,
            castDistance = 3f,
            radius = 0.5f,
            stepCount = 8,
            angle = 120f,
            offset = 0f,
            stepSize = 0.5f,
            distanceOffset = 0f,
            size = Vector2.one,
        };
    }
}
