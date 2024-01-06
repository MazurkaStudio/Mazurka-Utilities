using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheMazurkaStudio.Editor
{
    /// <summary>
    /// Make sure preload is alway start before other scenes
    /// </summary>
    public static class PlayModeSceneManager
    {
        private static bool isCurrentlyEnteringPlayMode; //Avoid start play mode if already click in editor
        
        [InitializeOnLoadMethod] //Call once at unity start
        private static void Onload() => EditorApplication.playModeStateChanged += PlayerModeStateChanged;
        

        //On Click Play in Editor
        private static void PlayerModeStateChanged(PlayModeStateChange mode)
        {
            if (!PlayModeSceneSettings.instance.IsEnable) return;
            if (string.IsNullOrEmpty(PlayModeSceneSettings.instance.GetPreloadScene))
            {
                Debug.LogError("Preload Scene is empty");
                return;
            }
            if (!PlayModeSceneSettings.instance.SceneExist)
            {
                Debug.LogError("Can't found scene for preload at " + PlayModeSceneSettings.instance.GetPreloadScene);
                return;
            }

            
            switch (mode)
            {
                //If going to play mode
                case PlayModeStateChange.ExitingEditMode:
                    isCurrentlyEnteringPlayMode = true;
                    SetCurrentFirstScene();
                    break;
                
                case PlayModeStateChange.ExitingPlayMode:
                    isCurrentlyEnteringPlayMode = false;
                    break;
                case PlayModeStateChange.EnteredEditMode:
                    ReloadLastOpenScenes();
                    break;
            }
        }
        
        
        [MenuItem("TheMazurkaStudio/Preload System/Launch Game")]
        public static void SetCurrentFirstScene()
        {
            PlayModeSceneSettings.instance.lastActiveScene= SceneManager.GetActiveScene().name;
            
            //Try found preload in scenes
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                
                var path = scene.path;

                //if current scene is not preload next
                if (path != PlayModeSceneSettings.instance.GetPreloadScene) continue; 
                
                //if not loaded equal to scene not loaded at all
                if (!scene.isLoaded) break;

                //else just play with preload as active scene!
                SceneManager.SetActiveScene(scene);
                if(!isCurrentlyEnteringPlayMode) EditorApplication.EnterPlaymode();
                return;
            }

            PlayModeSceneSettings.instance.shouldClosePreload = true;
            
            //We need load the scene and set active before play
            EditorSceneManager.sceneOpened += OnPreloadSceneOpened;
            EditorSceneManager.OpenScene(PlayModeSceneSettings.instance.GetPreloadScene, OpenSceneMode.Additive);
        }

        private static void OnPreloadSceneOpened(Scene scene, OpenSceneMode mode)
        {
            EditorSceneManager.sceneOpened -= OnPreloadSceneOpened;
            
            //Now we can start the game
            SceneManager.SetActiveScene(scene);
            if(!isCurrentlyEnteringPlayMode) EditorApplication.EnterPlaymode();
        }
        
        private static void ReloadLastOpenScenes()
        {
            //unload preload if need
            if (PlayModeSceneSettings.instance.shouldClosePreload)
            {
                PlayModeSceneSettings.instance.shouldClosePreload= false;

                for (var i = 0; i < SceneManager.sceneCount; i++)
                {
                    var scene = SceneManager.GetSceneAt(i);
                 
                    if (scene.path != PlayModeSceneSettings.instance.GetPreloadScene) continue;
                    EditorSceneManager.CloseScene(scene, true);
                    break;
                }
            }
            
            //Set active last active scene
            var targetSceneName = PlayModeSceneSettings.instance.lastActiveScene;
            
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                
                if (scene.name != targetSceneName) continue;
                
                SceneManager.SetActiveScene(scene);
                break;
            }
        }
    }
}
