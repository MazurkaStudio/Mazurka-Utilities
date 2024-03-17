using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheMazurkaStudio.Utilities.InteractionSystem
{
    [Serializable]
    public static class InteractionSystemDebugger
    {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        public static void Load()
        {
            if (EditorPrefs.HasKey("DEBUG_INTERACTION_SYSTEM"))
            {
                DEBUG_INTERACTION_SYSTEM = EditorPrefs.GetBool("DEBUG_INTERACTION_SYSTEM");
            }
            EditorApplication.quitting += Save;
        }
        
        [MenuItem("MazurkaStudio/Debug/Toggle Interaction System Gizmos")]
        public static void Toggle()
        {
            DEBUG_INTERACTION_SYSTEM = !DEBUG_INTERACTION_SYSTEM;
            Save();
        }
        
        
        public static void Save()
        {
            EditorPrefs.SetBool("DEBUG_INTERACTION_SYSTEM", DEBUG_INTERACTION_SYSTEM);
            EditorApplication.quitting -= Save;
        }
#endif
        
        public static bool DEBUG_INTERACTION_SYSTEM;
    }
}
