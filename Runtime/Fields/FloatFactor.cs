using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    public class FloatFactor
    {
        /// <summary>
        /// Initial value represent the value return when no factor has been add
        /// </summary>
        /// <param name="initialValue"></param>
        public FloatFactor(float initialValue = 1f)
        {
            this.initialValue = initialValue;
            Value = initialValue;
        }

        public float Value { get; private set; }

        public void AddFactor(float value)
        {
            factorValues.Add(value);
            Value = GetTotal;
        }

        public void RemoveFactor(float value)
        {
            factorValues.Remove(value);
            Value = GetTotal;
        }

        public void Clear()
        {
            factorValues.Clear();
            Value = GetTotal;
        }
        
        private readonly float initialValue;
        private readonly HashSet<float> factorValues = new();

        private float GetTotal => factorValues.Aggregate(initialValue, (current, value) => current * value);
    }
}
