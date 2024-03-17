using System;
using UnityEngine;

namespace TheMazurkaStudio.Utilities.Sensors
{
    public static partial class SensorUtils 
    {
        public static Vector2 AngleToVector(float angleDeg) => new(Mathf.Sin(Mathf.Deg2Rad * angleDeg), Mathf.Cos(Mathf.Deg2Rad * angleDeg));
        public static Vector2 ToVector(this float angleDeg) => new(Mathf.Sin(Mathf.Deg2Rad * angleDeg), Mathf.Cos(Mathf.Deg2Rad * angleDeg));
    }
}
