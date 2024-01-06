using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    [System.Serializable]
    public class BufferValue
    {
        public BufferValue() { }

        public BufferValue(float bufferTime)
        {
            SetBufferValue(bufferTime);
        }
            
        [SerializeField] private float _bufferTime;
            
        private float _lastTimeTrigger = float.MinValue;

        public bool IsInBuffer => Time.time - _lastTimeTrigger <= _bufferTime;
        public float RemainingTime => _bufferTime  - (Time.time - _lastTimeTrigger);
        public float Elapsed => Time.time - _lastTimeTrigger;
            
        /// <summary>
        /// Start buffer (on input press)
        /// </summary>
        public void Trigger() =>  _lastTimeTrigger = Time.time;
        
        /// <summary>
        /// Close the buffer
        /// </summary>
        public void Use() => _lastTimeTrigger = float.MinValue;
        public void SetBufferValue(float bufferTime) => _bufferTime = bufferTime;
    }
}
