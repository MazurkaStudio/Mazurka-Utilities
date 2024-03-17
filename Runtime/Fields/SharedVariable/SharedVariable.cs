using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// Overriden value has a default value that never change (only if you set up the value). But the Value field wil return
    /// the oldest overrider value if exist else the default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SharedVariable<T> where T : IComparable, IConvertible
    {
        [SerializeField] private T defaultValue;
        [SerializeField] private T currentValue;
        [SerializeField] private bool isOverride;
        [SerializeField] private UnityEvent<T> onValueChanged;

        private SharedVariableOverrider<T> overridenBy;
        private readonly HashSet <SharedVariableOverrider<T>> overrideQueue = new();
        
        public SharedVariable(T defaultValue, UnityAction<T> onValueChanged = null)
        {
            this.defaultValue = defaultValue;
            this.isOverride = false;
            this.currentValue = this.defaultValue;
            this.onValueChanged = new UnityEvent<T>();
            if(onValueChanged != null) this.onValueChanged.AddListener(onValueChanged);
        }

        public T Value => currentValue;

        public void SetDefaultValue(T newValue)
        {
            defaultValue = newValue;
            if (!isOverride) ValueChanged(defaultValue);
        }

        public void AddListener(UnityAction<T> callback) =>  onValueChanged.AddListener(callback);
        public void RemoveListener(UnityAction<T> callback) =>  onValueChanged.RemoveListener(callback);
 
        
        public bool TryOverride(SharedVariableOverrider<T> overrider)
        {
            //FIRST TO OVERRIDE
            if (!isOverride)
            {
                StartOverride(overrider);
                return true;
            }

            //ELSE ADD IF NOT IN LIST
            if (overrideQueue.Contains(overrider)) return false;
            overrideQueue.Add(overrider);
            
            return true;
        }
        public void Release(SharedVariableOverrider<T> overrider)
        {
            //JUST REMOVE FROM LIST
            if (overridenBy != overrider)
            {
                if (overrideQueue.Contains(overrider)) overrideQueue.Remove(overrider);
            }
            //TRY OVERRIDE BY NEXT
            else
            {
                if (overrideQueue.Count <= 0)
                {
                    StopOverride();
                    return;
                }
                
                StartOverride(overrideQueue.First());
            }
        }

        
        
        private void StartOverride(SharedVariableOverrider<T> overrider)
        {
            overridenBy = overrider;
            overrideQueue.Remove(overrider);
            isOverride = true;
            ValueChanged(overridenBy.Value);
        }
        private void StopOverride()
        {
            if (!isOverride) return;
            
            overridenBy = null;
            isOverride = false;
            ValueChanged(defaultValue);
        }

        private void ValueChanged(T newValue)
        {
            currentValue = newValue;
            onValueChanged?.Invoke(currentValue);
        }
    }
    
#if UNITY_EDITOR
    
    [UnityEditor.CustomPropertyDrawer(typeof(SharedVariable<>))]
    public class SharedVariableDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            UnityEditor.EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = UnityEditor.EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't indent child fields
            var indent = UnityEditor.EditorGUI.indentLevel;
            UnityEditor.EditorGUI.indentLevel = 0;

            // Calculate rects
            var amountRect = new Rect(position.x, position.y, 100, position.height);
            var nameRect = new Rect(position.x + 110, position.y, 100, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            var isOverride = property.FindPropertyRelative("isOverride").boolValue;
            
            if (isOverride)
            {
                GUI.enabled = false;
                UnityEditor.EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("defaultValue"), GUIContent.none);
                UnityEditor.EditorGUI.PropertyField(nameRect,  property.FindPropertyRelative("currentValue"), GUIContent.none);
                GUI.enabled = true;
            }
            else
            {
                UnityEditor.EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("defaultValue"), GUIContent.none);
            }
            
            // Set indent back to what it was
            UnityEditor.EditorGUI.indentLevel = indent;

            UnityEditor.EditorGUI.EndProperty();
        }
    }
#endif
}
