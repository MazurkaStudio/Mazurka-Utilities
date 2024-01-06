using System;
using UnityEngine;
using UnityEngine.Events;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// Send events when value changed to any listeners.
    /// Can be serialize if not static.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Observer<T>
    {
        [SerializeField] private T value;
        [SerializeField] private UnityEvent<T> onValueChanged;

        private bool isMute;
        private bool muteOnce;
        
        public T Value
        {
            get => value;
            set => Set(value);
        }

        public Observer(T value, UnityAction<T> callback = null)
        {
            this.value = value;
            onValueChanged = new UnityEvent<T>();
            if(callback != null) onValueChanged.AddListener(callback);
        }

        private void Set(T newValue)
        {
            if (Equals((value, newValue))) return;
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
            onValueChanged.Invoke(value);
        }

        
        
        public void AddListener(UnityAction<T> callback)
        {
            if (callback == null) return;
            onValueChanged ??= new UnityEvent<T>();
            onValueChanged.AddListener(callback);
        }
        public void RemoveListener(UnityAction<T> callback)
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
