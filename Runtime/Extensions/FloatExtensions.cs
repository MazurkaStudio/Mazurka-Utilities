using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    public static class FloatExtensions
    {
        public static Vector2 AngleToVector(float angleDeg) => new(Mathf.Sin(Mathf.Deg2Rad * angleDeg), Mathf.Cos(Mathf.Deg2Rad * angleDeg));
        public static Vector2 ToAngleVector(this float angleDeg) => new(Mathf.Sin(Mathf.Deg2Rad * angleDeg), Mathf.Cos(Mathf.Deg2Rad * angleDeg));
    }
}
