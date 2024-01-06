using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TheMazurkaStudio.Editor
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
            var scenes= EditorBuildSettings.scenes.Select(x => x.path).ToArray();
            // One line of  oxygen free code.
            property.intValue = EditorGUI.Popup(position, label.text,  property.intValue, scenes);
        }
    }
}
