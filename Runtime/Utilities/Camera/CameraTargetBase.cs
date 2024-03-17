using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    public class CameraTargetBase : MonoBehaviour, ICameraTarget
    {
        [SerializeField] private Transform cameraTarget;

        protected virtual void Awake()
        {
            if (cameraTarget == null) cameraTarget = transform;
        }

        private void Reset()
        {
            cameraTarget = transform;
        }

        public Transform CameraTarget => cameraTarget;
    }
}
