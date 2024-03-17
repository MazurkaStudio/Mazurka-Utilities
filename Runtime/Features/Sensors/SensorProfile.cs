using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stump.TheMazurkaStudio.Utilities
{
    [CreateAssetMenu(menuName = "Stump/Sensors/New Sensor Profile")]
    public class SensorProfile : ScriptableObject
    {
        public Color color;
        public LayerMask seekLayer;
    }
}
