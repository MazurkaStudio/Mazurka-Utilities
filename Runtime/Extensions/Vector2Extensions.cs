using System;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Snaps the input Vector2 to the nearest angle, starting from Vector2.right and circling counterclockwise
        /// </summary>
        /// <param name="vector">The vector to be processed</param>
        /// <param name="increments">Number of increments (recommended: power of 2, value of 4 or greater)</param>
        /// <returns></returns>
        public static Vector2 SnapAngle(this Vector2 vector, int increments)
        {
            float angle = Mathf.Atan2(vector.y, vector.x);
            float direction = ((angle / Mathf.PI) + 1) * 0.5f; // Convert to [0..1] range from [-pi..pi]
            float snappedDirection = Mathf.Round(direction * increments) / increments; // Snap to increment
            snappedDirection = ((snappedDirection * 2) - 1) * Mathf.PI; // Convert back to [-pi..pi] range
            Vector2 snappedVector = new Vector2(Mathf.Cos(snappedDirection), Mathf.Sin(snappedDirection));
            return vector.magnitude * snappedVector;
        }

        public static Vector2 ClampMagnitude(this Vector2 vector, float clampValue)
        {
            return Vector2.ClampMagnitude(vector, clampValue);
        }
        
        public static Vector2 BounceVector(this Vector2 velocity, Vector2 surfaceNormal)
        {
            return Vector2.Reflect(velocity, surfaceNormal);
        }
        
        public static Vector2 With(this Vector2 vector, float? x = null, float? y = null) => new(x ?? vector.x, y ?? vector.y);
        
        public static Vector2 Add(this Vector2 vector, float? x = null, float? y = null) => new(vector.x + (x ?? 0), vector.y + (y ?? 0));
        
        public static Vector2 GetClosestVector(this Vector2 vector, Vector2[] otherVectors)
        {
            if (otherVectors.Length == 0) throw new Exception("The list of other vectors is empty");
            var minDistance = Vector2.Distance(vector, otherVectors[0]);
            var minVector = otherVectors[0];
            for (var i = otherVectors.Length - 1; i > 0; i--)
            {
                var newDistance = Vector2.Distance(vector, otherVectors[i]);
                if (newDistance < minDistance)
                {
                    minDistance = newDistance;
                    minVector = otherVectors[i];
                }
            }
            return minVector;
        }
        
        public static Quaternion LookAt2D(this Vector2 rightDir) => Quaternion.Euler(0f, 0f, Mathf.Atan2(rightDir.y, rightDir.x) * Mathf.Rad2Deg);
        
        public static float HorizontalDistance(this Vector2 from, Vector2 to) => Mathf.Abs(from.x - to.x);
        public static float VerticalDistance(this Vector2 from, Vector2 to) => Mathf.Abs(from.y - to.y);
        public static float HorizontalDistance(this Vector2 from, Vector3 to) => Mathf.Abs(from.x - to.x);
        public static float VerticalDistance(this Vector2 from, Vector3 to) => Mathf.Abs(from.y - to.y);
        public static float DepthDistance(this Vector2 from, Vector3 to) => Mathf.Abs(0f - to.z);
    }
}
