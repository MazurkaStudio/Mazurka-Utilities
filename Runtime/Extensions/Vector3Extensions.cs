using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    public static class Vector3Extensions
    {
        public static float HorizontalDistance(this Vector3 from, Vector3 to) => Mathf.Abs(from.x - to.x);
        public static float VerticalDistance(this Vector3 from, Vector3 to) => Mathf.Abs(from.y - to.y);
        public static float DepthDistance(this Vector3 from, Vector3 to) => Mathf.Abs(from.z - to.z);
        public static float HorizontalDistance(this Vector3 from, Vector2 to) => Mathf.Abs(from.x - to.x);
        public static float VerticalDistance(this Vector3 from, Vector2 to) => Mathf.Abs(from.y - to.y);
        public static float DepthDistance(this Vector3 from, Vector2 to) => Mathf.Abs(from.z - 0f);
        
        public static float Distance2D(this Vector3 from, Vector3 to) => Vector2.Distance(from, to);
        
        public static Quaternion LookAt2D(this Vector3 rightDir) => Quaternion.Euler(0f, 0f, Mathf.Atan2(rightDir.y, rightDir.x) * Mathf.Rad2Deg);
    }
}
