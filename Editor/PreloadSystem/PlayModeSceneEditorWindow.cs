using UnityEditor;
using UnityEngine;

namespace TheMazurkaStudio.Editor
{
    public class PlayModeSceneEditorWindow : EditorWindow
    {
        [MenuItem("TheMazurkaStudio/Preload System/Set Preload Scene Path")]
        public static void SetPreloadScenePath()
        {
            var path = EditorUtility.OpenFilePanel("Preload Scene Path", "", "unity");
            var split = path.Split("/Assets/");
            path = "Assets/" + split[1];
            PlayModeSceneSettings.instance.SetPreloadScenePath(path);
        } 
        
        [MenuItem("TheMazurkaStudio/Preload System/Enable Preload Scene System")]
        public static void Enable()
        {
            PlayModeSceneSettings.instance.Enable(true);
        } 
        
        [MenuItem("TheMazurkaStudio/Preload System/Disable Preload Scene System")]
        public static void Disable()
        {
            PlayModeSceneSettings.instance.Enable(false);
        } 
    }
}

