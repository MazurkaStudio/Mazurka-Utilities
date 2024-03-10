using System;
using UnityEngine;

public static class GizmosExtensions
{
    /// <summary>
    /// Draws a wire cube with a given rotation 
    /// </summary>
    /// <param name="center"></param>
    /// <param name="size"></param>
    /// <param name="rotation"></param>
    public static void DrawWireCube(Vector3 center, Vector3 size, Quaternion rotation = default(Quaternion))
    {
        var old = Gizmos.matrix;
        if (rotation.Equals(default(Quaternion)))
            rotation = Quaternion.identity;
        Gizmos.matrix = Matrix4x4.TRS(center, rotation, size);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = old;
    }

    public static void DrawArrow(Vector3 from, Vector3 to, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.DrawLine(from, to);
        var direction = to - from;
        var right = Quaternion.LookRotation(direction) * Quaternion.Euler(180 + arrowHeadAngle, 0, 0) *
                    new Vector3(0, 0, 1);
        var left = Quaternion.LookRotation(direction) * Quaternion.Euler(180 - arrowHeadAngle, 0, 0) *
                   new Vector3(0, 0, 1);
        Gizmos.DrawLine(to, to + right * arrowHeadLength);
        Gizmos.DrawLine(to, to + left * arrowHeadLength);
    }

    public static void DrawWireSphere(Vector3 center, float radius, Quaternion rotation = default(Quaternion))
    {
        var old = Gizmos.matrix;
        if (rotation.Equals(default(Quaternion)))
            rotation = Quaternion.identity;
        Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
        Gizmos.DrawWireSphere(Vector3.zero, radius);
        Gizmos.matrix = old;
    }


    public static void DrawCircleCast(Vector3 origin, Vector3 direction, float radius)
    {
        var tar = origin + direction;
        Vector3 crossVector = Vector2.Perpendicular(direction.normalized) * radius;

        Gizmos.DrawLine(origin + crossVector, tar + crossVector);
        Gizmos.DrawLine(origin - crossVector, tar - crossVector);

        var lookAt = Quaternion.LookRotation(crossVector.normalized, Vector3.forward);
        DrawWireCircle(origin, radius, 24, lookAt);

        lookAt = Quaternion.LookRotation(-crossVector.normalized, Vector3.forward);
        DrawWireCircle(tar, radius, 24, lookAt);
    }

    /// <summary>
    /// Draws a flat wire circle (up)
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <param name="segments"></param>
    /// <param name="rotation"></param>
    public static void DrawWireCircle(Vector3 center, float radius, int segments = 20, Quaternion rotation = default(Quaternion))
    {
        DrawWireArc(center, radius, 360, segments, rotation);
    }

    /// <summary>
    /// Draws an arc with a rotation around the center
    /// </summary>
    /// <param name="center">center point</param>
    /// <param name="radius">radiu</param>
    /// <param name="angle">angle in degrees</param>
    /// <param name="segments">number of segments</param>
    /// <param name="rotation">rotation around the center</param>
    public static void DrawWireArc(Vector3 center, float radius, float angle, int segments = 20, Quaternion rotation = default(Quaternion))
    {
        var old = Gizmos.matrix;

        Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
        Vector3 from = Vector3.forward * radius;
        var step = Mathf.RoundToInt(angle / segments);
        for (int i = 0; i <= angle; i += step)
        {
            var to = new Vector3(radius * Mathf.Sin(i * Mathf.Deg2Rad), 0, radius * Mathf.Cos(i * Mathf.Deg2Rad));
            Gizmos.DrawLine(from, to);
            from = to;
        }

        Gizmos.matrix = old;
    }


    /// <summary>
    /// Draws an arc with a rotation around an arbitraty center of rotation
    /// </summary>
    /// <param name="center">the circle's center point</param>
    /// <param name="radius">radius</param>
    /// <param name="angle">angle in degrees</param>
    /// <param name="segments">number of segments</param>
    /// <param name="rotation">rotation around the centerOfRotation</param>
    /// <param name="centerOfRotation">center of rotation</param>
    public static void DrawWireArc(Vector3 center, float radius, float angle, int segments, Quaternion rotation, Vector3 centerOfRotation)
    {
        var old = Gizmos.matrix;
        if (rotation.Equals(default(Quaternion)))
            rotation = Quaternion.identity;
        Gizmos.matrix = Matrix4x4.TRS(centerOfRotation, rotation, Vector3.one);
        var deltaTranslation = centerOfRotation - center;
        Vector3 from = deltaTranslation + Vector3.forward * radius;
        var step = Mathf.RoundToInt(angle / segments);
        for (int i = 0; i <= angle; i += step)
        {
            var to = new Vector3(radius * Mathf.Sin(i * Mathf.Deg2Rad), 0, radius * Mathf.Cos(i * Mathf.Deg2Rad)) +
                     deltaTranslation;
            Gizmos.DrawLine(from, to);
            from = to;
        }

        Gizmos.matrix = old;
    }

    /// <summary>
    /// Draws an arc with a rotation around an arbitraty center of rotation
    /// </summary>
    /// <param name="matrix">Gizmo matrix applied before drawing</param>
    /// <param name="radius">radius</param>
    /// <param name="angle">angle in degrees</param>
    /// <param name="segments">number of segments</param>
    public static void DrawWireArc(Matrix4x4 matrix, float radius, float angle, int segments)
    {
        var old = Gizmos.matrix;
        Gizmos.matrix = matrix;
        Vector3 from = Vector3.forward * radius;
        var step = Mathf.RoundToInt(angle / segments);
        for (int i = 0; i <= angle; i += step)
        {
            var to = new Vector3(radius * Mathf.Sin(i * Mathf.Deg2Rad), 0, radius * Mathf.Cos(i * Mathf.Deg2Rad));
            Gizmos.DrawLine(from, to);
            from = to;
        }

        Gizmos.matrix = old;
    }

    /// <summary>
    /// Draws a wire cylinder face up with a rotation around the center
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <param name="height"></param>
    /// <param name="rotation"></param>
    public static void DrawWireCylinder(Vector3 center, float radius, float height, Quaternion rotation = default(Quaternion))
    {
        var old = Gizmos.matrix;
        if (rotation.Equals(default(Quaternion)))
            rotation = Quaternion.identity;
        Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
        var half = height / 2;

        //draw the 4 outer lines
        Gizmos.DrawLine(Vector3.right * radius - Vector3.up * half, Vector3.right * radius + Vector3.up * half);
        Gizmos.DrawLine(-Vector3.right * radius - Vector3.up * half, -Vector3.right * radius + Vector3.up * half);
        Gizmos.DrawLine(Vector3.forward * radius - Vector3.up * half, Vector3.forward * radius + Vector3.up * half);
        Gizmos.DrawLine(-Vector3.forward * radius - Vector3.up * half, -Vector3.forward * radius + Vector3.up * half);

        //draw the 2 cricles with the center of rotation being the center of the cylinder, not the center of the circle itself
        DrawWireArc(center + Vector3.up * half, radius, 360, 20, rotation, center);
        DrawWireArc(center + Vector3.down * half, radius, 360, 20, rotation, center);
        Gizmos.matrix = old;
    }

    /// <summary>
    /// Draws a wire capsule face up
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <param name="height"></param>
    /// <param name="rotation"></param>
    public static void DrawWireCapsule(Vector3 center, float radius, float height, Quaternion rotation = default(Quaternion))
    {
        if (rotation.Equals(default(Quaternion)))
            rotation = Quaternion.identity;
        var old = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
        var half = height / 2 - radius;

        //draw cylinder base
        DrawWireCylinder(center, radius, height - radius * 2, rotation);

        //draw upper cap
        //do some cool stuff with orthogonal matrices
        var mat = Matrix4x4.Translate(center + rotation * Vector3.up * half) *
                  Matrix4x4.Rotate(rotation * Quaternion.AngleAxis(90, Vector3.forward));
        DrawWireArc(mat, radius, 180, 20);
        mat = Matrix4x4.Translate(center + rotation * Vector3.up * half) * Matrix4x4.Rotate(rotation *
            Quaternion.AngleAxis(90, Vector3.up) * Quaternion.AngleAxis(90, Vector3.forward));
        DrawWireArc(mat, radius, 180, 20);

        //draw lower cap
        mat = Matrix4x4.Translate(center + rotation * Vector3.down * half) * Matrix4x4.Rotate(rotation *
            Quaternion.AngleAxis(90, Vector3.up) * Quaternion.AngleAxis(-90, Vector3.forward));
        DrawWireArc(mat, radius, 180, 20);
        mat = Matrix4x4.Translate(center + rotation * Vector3.down * half) *
              Matrix4x4.Rotate(rotation * Quaternion.AngleAxis(-90, Vector3.forward));
        DrawWireArc(mat, radius, 180, 20);

        Gizmos.matrix = old;
    }

    public static void DrawConeCast(Vector3 origin, Vector3 direction, float radius, float angle, int segments)
    {
        var tar = origin + direction;
        Vector3 crossVector = Vector2.Perpendicular(direction.normalized) * radius;
        var lookAt = Quaternion.LookRotation(crossVector.normalized, Vector3.forward);
        DrawWireArc(origin, radius, angle, segments, lookAt);
    }

    
    public static class BoxCastDrawer
    {
        /// <summary>
        ///     Visualizes BoxCast with help of debug lines.
        /// </summary>
        /// <param name="origin"> The point in 2D space where the box originates. </param>
        /// <param name="size"> The size of the box. </param>
        /// <param name="angle"> The angle of the box (in degrees). </param>
        /// <param name="direction"> A vector representing the direction of the box. </param>
        /// <param name="distance"> The maximum distance over which to cast the box. </param>
        public static void Draw(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance = Mathf.Infinity)
        {
            // Set up points to draw the cast.
            Vector2[] originalBox = CreateOriginalBox(origin, size, angle);

            Vector2 distanceVector = GetDistanceVector(distance, direction);
            Vector2[] shiftedBox = CreateShiftedBox(originalBox, distanceVector);
            
            DrawBox(originalBox);
            DrawBox(shiftedBox);
            ConnectBoxes(originalBox, shiftedBox);
        }
        
        private static Vector2[] CreateOriginalBox(Vector2 origin, Vector2 size, float angle)
        {
            var w = size.x * 0.5f;
            var h = size.y * 0.5f;
            var q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));

            var box = new Vector2[4]
            {
                new (-w, h),
                new (w, h),
                new (w, -h),
                new (-w, -h),
            };

            for (int i = 0; i < 4; i++)
            {
                box[i] = (Vector2)(q * box[i]) + origin;
            }

            return box;
        }

        private static Vector2[] CreateShiftedBox(Vector2[] box, Vector2 distance)
        {
            var shiftedBox = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                shiftedBox[i] = box[i] + distance;
            }

            return shiftedBox;
        }

        private static void DrawBox(Vector2[] box)
        {
            Gizmos.DrawLine(box[0], box[1]);
            Gizmos.DrawLine(box[1], box[2]);
            Gizmos.DrawLine(box[2], box[3]);
            Gizmos.DrawLine(box[3], box[0]);
        }

        private static void ConnectBoxes(Vector2[] firstBox, Vector2[] secondBox)
        {
            Gizmos.DrawLine(firstBox[0], secondBox[0]);
            Gizmos.DrawLine(firstBox[1], secondBox[1]);
            Gizmos.DrawLine(firstBox[2], secondBox[2]);
            Gizmos.DrawLine(firstBox[3], secondBox[3]);
        }

        private static Vector2 GetDistanceVector(float distance, Vector2 direction)
        {
            if (float.IsPositiveInfinity(distance))
            {
                // Draw some large distance e.g. 5 scene widths long.
                var sceneWidth = Camera.main.orthographicSize * Camera.main.aspect * 2f;
                distance = sceneWidth * 5f;
            }

            return direction.normalized * distance;
        }
    }
}