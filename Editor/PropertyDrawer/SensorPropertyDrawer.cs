using TheMazurkaStudio.Utilities.Sensors;
using UnityEditor;
using UnityEngine;

namespace TheMazurkaStudio.Utilities.Editor.Sensors
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Sensor<>))]
    public class SensorsPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SensorDrawerUtils.DrawProperty(position, property, label);
        }
    }
    
    [CustomPropertyDrawer(typeof(Sensor))]
    public class SensorPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SensorDrawerUtils.DrawProperty(position, property, label);
        }
    }
    
    [CustomPropertyDrawer(typeof(SensorParameters))]
    public class SensorParametersPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SensorDrawerUtils.DrawParameters(position, property, label);
        }
    }
    
    public static class SensorDrawerUtils
    {
        public static void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.LabelField(label);
            
            var profile = property.FindPropertyRelative("profile");
            var debugColor = property.FindPropertyRelative("debugColor");
            var transform = property.FindPropertyRelative("transform");
            var transformTarget = property.FindPropertyRelative("transformTarget");
            var maxCastCount = property.FindPropertyRelative("maxCastCount");
            var maxCastCountPerRay = property.FindPropertyRelative("maxCastCountPerRay");
            var seekLayer = property.FindPropertyRelative("seekLayer");
            var parameters = property.FindPropertyRelative("parameters");
            
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.PropertyField(profile);
           
            if (profile.objectReferenceValue != null)
            {
                if (GUILayout.Button("Remove profile"))
                {
                    profile.objectReferenceValue = null;
                }
                
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(debugColor);
                EditorGUILayout.PropertyField(seekLayer);
            }
            
            EditorGUILayout.PropertyField(transform);
            EditorGUILayout.PropertyField(maxCastCountPerRay);
            EditorGUILayout.PropertyField(maxCastCount);
           
            
            EditorGUILayout.Space(10f);
            
            EditorGUILayout.PropertyField(parameters);
   
            EditorGUILayout.EndVertical();
            
            EditorGUI.EndProperty();
        }
        
        public static void DrawParameters(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.LabelField(label);
            
            
            var castOffset = property.FindPropertyRelative("castOffset");
            var seekMethode = property.FindPropertyRelative("seekMethode");
            var castDistance = property.FindPropertyRelative("castDistance");
            var radius = property.FindPropertyRelative("radius");
            var stepCount = property.FindPropertyRelative("stepCount");
            var angle = property.FindPropertyRelative("angle");
            var offset = property.FindPropertyRelative("offset");
            var stepSize = property.FindPropertyRelative("stepSize");
            var distanceOffset = property.FindPropertyRelative("distanceOffset");
            var size = property.FindPropertyRelative("size");
          
            
            EditorGUILayout.PropertyField(castOffset);
            EditorGUILayout.PropertyField(seekMethode);
            
            EditorGUILayout.Space(10f);
            
            var lastType = (SeekMethode)seekMethode.intValue;
            
            switch (lastType)
            {
                case SeekMethode.None:
                    break;
                case SeekMethode.Raycast:
                {
                    EditorGUILayout.PropertyField(castDistance);
                    
                    if (GUILayout.Button("Reset Values"))
                    {
                        castDistance.floatValue = 5f;
                    }
                    break;
                }
                case SeekMethode.CircleCast:
                {
                    EditorGUILayout.PropertyField(castDistance);
                    EditorGUILayout.PropertyField(radius);
                    
                    if (GUILayout.Button("Reset Values"))
                    {
                        castDistance.floatValue = 3f;
                        radius.floatValue = .25f;
                    }
                    break; 
                }
                case SeekMethode.BoxCast:
                {
                    EditorGUILayout.PropertyField(castDistance);
                    EditorGUILayout.PropertyField(angle);
                    EditorGUILayout.PropertyField(size);
                    
                    if (GUILayout.Button("Reset Values"))
                    {
                        castDistance.floatValue = 3f;
                        angle.floatValue = 0f;
                        size.vector2Value = Vector2.one;
                    }
                    break; 
                }

                case SeekMethode.OverlapSphere:
                {
                    EditorGUILayout.PropertyField(radius);
                    
                    if (GUILayout.Button("Reset Values"))
                    {
                        radius.floatValue = 5f;
                    }
                    break;
                }

                case SeekMethode.Burst:
                {
                    EditorGUILayout.PropertyField(castDistance);
                    EditorGUILayout.PropertyField(stepCount);
                    EditorGUILayout.PropertyField(angle);
                    EditorGUILayout.PropertyField(offset);
                    EditorGUILayout.PropertyField(distanceOffset);
                    
                    if (GUILayout.Button("Reset Values"))
                    {
                        castDistance.floatValue = 3f;
                        stepCount.intValue = 12;
                        angle.floatValue = 270f;
                        offset.floatValue = 0f;
                        distanceOffset.floatValue = 1f;
                    }
                    break;
                }

                case SeekMethode.Ladder:
                {
                    EditorGUILayout.PropertyField(castDistance);
                    EditorGUILayout.PropertyField(stepCount);
                    EditorGUILayout.PropertyField(angle);
                    EditorGUILayout.PropertyField(stepSize);

                    if (GUILayout.Button("Reset Values"))
                    {
                        castDistance.floatValue = 1f;
                        stepCount.intValue = 8;
                        angle.floatValue = 45f;
                        stepSize.floatValue = .25f;
                    }
                    break;
                }
            }
            
            EditorGUILayout.EndVertical();
            
            EditorGUI.EndProperty();
        }
    }
#endif
}
