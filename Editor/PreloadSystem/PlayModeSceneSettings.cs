using UnityEditor;
using UnityEngine;

namespace TheMazurkaStudio.Utilities.Editor
{
    [FilePath("PreloadSystem/PlayModeSceneSettings.mazurka", FilePathAttribute.Location.PreferencesFolder)]
    public class PlayModeSceneSettings : ScriptableSingleton<PlayModeSceneSettings>
    {
        [SerializeField] private string _preloadScenePath = "Scene/Preload";
        [SerializeField] private bool _enable = false;
        [HideInInspector] public bool shouldClosePreload;
        [HideInInspector]public string lastActiveScene;
        public string GetPreloadScene => _preloadScenePath;
        public bool IsEnable => _enable;
        public bool SceneExist => AssetDatabase.LoadAssetAtPath<SceneAsset>(GetPreloadScene) != null;

        public void SetPreloadScenePath(string path)
        {
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);

            if (sceneAsset == null)
            {
                Debug.Log("Scene can't be found at " + path);
                return;
            }
            
            Debug.Log("Set Preload Scene at " + path);

            _preloadScenePath = path;

            //AddToBuildSettings(_preloadScenePath);
        }

        private void AddToBuildSettings(string path)
        {
            var original = EditorBuildSettings.scenes;
            
            var newSettings = new EditorBuildSettingsScene[original.Length + 1]; 
            
            System.Array.ConstrainedCopy(original, 0,newSettings, 1, original.Length); 

            var sceneToAdd = new EditorBuildSettingsScene(path, true); 

            newSettings[0] = sceneToAdd; 

            EditorBuildSettings.scenes = newSettings;
        }

        public void Enable(bool value)
        {
            _enable = value;
        }
        
        public string Path => GetFilePath();
    }
}
