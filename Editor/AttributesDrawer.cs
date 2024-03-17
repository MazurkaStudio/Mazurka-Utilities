using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TheMazurkaStudio.Utilities.Editor
{
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerAttributeEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // One line of  oxygen free code.
            property.intValue = EditorGUI.LayerField(position, label,  property.intValue);
        }
    }
    
    [CustomPropertyDrawer(typeof(ScenesDropDownAttribute))]
    public class ScenesDropDownAttributeEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var scenes = EditorBuildSettings.scenes.Select(x => x.path).ToArray();
            var sceneNames = EditorBuildSettings.scenes.Select(x => System.IO.Path.GetFileNameWithoutExtension(x.path)).ToArray();

            string currentScenePath = property.stringValue;

            int selectedIndex = Mathf.Max(0, Array.IndexOf(scenes, currentScenePath));

            // One line of oxygen-free code.
            selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, sceneNames);

            if (selectedIndex >= 0 && selectedIndex < scenes.Length)
            {
                property.stringValue = scenes[selectedIndex];
            }
        }
    }
}
