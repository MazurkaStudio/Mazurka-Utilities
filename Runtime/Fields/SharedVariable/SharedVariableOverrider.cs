using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// Serializable value with bool override state attach (similar to post process fields)
    /// Overrider keep a trace of the overriden values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SharedVariableOverrider<T> where T : IComparable, IConvertible
    {
        [SerializeField] private T value; //the value to set
        [SerializeField] private bool isOverride; //should override data
        
        private readonly HashSet<SharedVariable<T>> overrideTargets = new();//all the share data overriden by this

        public bool IsOverride => isOverride;
        public T Value => value;
        
        public SharedVariableOverrider(T value, bool isOverride)
        {
            this.value = value;
            this.isOverride = isOverride;
        }
        
        public void SetValue(T newValue) => value = newValue;
        
        public void BindWith(SharedVariable<T> field)
        {
            if (!isOverride) return;
            
            if (overrideTargets.Contains(field)) return;
            
            if (field.TryOverride(this)) overrideTargets.Add(field);
        }

        public void Unbind(SharedVariable<T> field)
        {
            if (!isOverride) return;
            
            if (!overrideTargets.Contains(field)) return;
            
            field?.Release(this);
            
            overrideTargets.Remove(field);
        }

        public void Dispose()
        {
            foreach (var field in overrideTargets)
            {
                field?.Release(this);
            }
            
            overrideTargets.Clear();
        }
    }
    
    
#if UNITY_EDITOR
    
    [UnityEditor.CustomPropertyDrawer(typeof(SharedVariableOverrider<>))]
    public class OverriderValueDrawer : UnityEditor.PropertyDrawer
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
        
            UnityEditor.EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("isOverride"), GUIContent.none);
            var isOverride = property.FindPropertyRelative("isOverride").boolValue;
            
            if (!isOverride)
            {
                GUI.enabled = false;
                UnityEditor.EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("value"), GUIContent.none);
                GUI.enabled = true;
            }
            else
            {
                UnityEditor.EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("value"), GUIContent.none);
            }
            // Set indent back to what it was
            UnityEditor.EditorGUI.indentLevel = indent;

            UnityEditor.EditorGUI.EndProperty();
        }
    }
    
#endif
}
