using System;
using UnityEngine;
using UnityEngine.Events;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// Send events when value changed to any listeners (value, lastValue)
    /// Can be serialize if not static.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ComplexObserver<T>
    {
        [SerializeField] private T value;
        private T lastValue;
        [SerializeField] private UnityEvent<T, T> onValueChanged;

        private bool isMute;
        private bool muteOnce;
        
        public T Value
        {
            get => value;
            set => Set(value);
        }

        public ComplexObserver(T value, UnityAction<T, T> callback = null)
        {
            this.value = value;
            this.lastValue = value;
            onValueChanged = new UnityEvent<T, T>();
            if(callback != null) onValueChanged.AddListener(callback);
        }

        private void Set(T newValue)
        {
            if (Equals((value, newValue))) return;
            lastValue = value;
            value = newValue;
            Invoke();
        }

        private void Invoke()
        {
            if (isMute || muteOnce)
            {
                muteOnce = false;
                return;
            }
            
            onValueChanged.Invoke(value, lastValue);
        }

        
        
        public void AddListener(UnityAction<T, T> callback)
        {
            if (callback == null) return;
            onValueChanged ??= new UnityEvent<T, T>();
            onValueChanged.AddListener(callback);
        }
        public void RemoveListener(UnityAction<T, T> callback)
        {
            if (callback == null) return;
            if (onValueChanged == null) return;
            onValueChanged.RemoveListener(callback);
        }
        public void RemoveAllListeners()
        {
            if (onValueChanged == null) return;
            onValueChanged.RemoveAllListeners();
        }

        
        public void Dispose()
        {
            RemoveAllListeners();
            onValueChanged = null;
            value = default;
        }
        
        public void Mute(bool mute) => isMute = mute;
        public void MuteOnce() => muteOnce = true;
    }
}