using TheMazurkaStudio.Utilities.Sensors;
using UnityEditor;
using UnityEngine;

namespace TheMazurkaStudio.Utilities.Editor
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(BufferValue))]
    public class BufferValuePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            
            
            var _bufferTime = property.FindPropertyRelative("_bufferTime");
            var _lastTimeTrigger = property.FindPropertyRelative("_lastTimeTrigger");
            
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField(label);
            
            EditorGUILayout.PropertyField(_bufferTime, GUIContent.none);
            
            GUI.enabled = false;
            EditorGUILayout.Toggle(Time.time - _lastTimeTrigger.floatValue < _bufferTime.floatValue);
            GUI.enabled = true;
            
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndProperty();
        }
    }
    
#endif
}
