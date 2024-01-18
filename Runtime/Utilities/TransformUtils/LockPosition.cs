using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    [ExecuteAlways]
    public class LockPosition : MonoBehaviour
    {
        [SerializeField] private bool isLock = true;
        [SerializeField] private bool localSpace = true;
        private Vector3 position;
        
        private void OnEnable()
        {
            if (transform.parent == null) localSpace = false;
            RecordPosition();
        }

        void Update()
        {
            if (!isLock)
            {
                RecordPosition();
                return;
            }
            
            if (transform.hasChanged)
            {
                if (localSpace) transform.localPosition = position;
                else transform.position = position;
                
                transform.hasChanged = false;
            }
        }

        void RecordPosition()
        {
            position = localSpace ? transform.localPosition : transform.position;
        }
    }
}
