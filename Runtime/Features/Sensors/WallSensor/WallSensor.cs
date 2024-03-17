using UnityEngine;

namespace TheMazurkaStudio.Utilities.Sensors
{
    public class WallSensor : MonoBehaviour
    {
        [SerializeField] private Sensor sensor;
        
        public bool HaveWall()
        {
            return sensor.SeekFirst();
        }
        
        private void OnDrawGizmosSelected()
        {
            sensor.DrawGizmos();
        }
    }
}
