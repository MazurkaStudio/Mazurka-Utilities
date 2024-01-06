using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheMazurkaStudio.Utilities
{
    public static class EventBusUtils 
    {
        
#if UNITY_EDITOR
        
    public static PlayModeStateChange PlayModeState { get; private set; }
    
    [InitializeOnLoadMethod]
    public static void InitializeEditor()
    {
        EditorApplication.playModeStateChanged -= EditorApplicationOnplayModeStateChanged;
        EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
    }

    private static void EditorApplicationOnplayModeStateChanged(PlayModeStateChange state)
    {
        PlayModeState = state;
        
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            ClearAllBuses();
        }
    }
    
#endif
        
        public static IReadOnlyList<Type> EventTypes { get; set; }
        public static IReadOnlyList<Type> EventBusTypes { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            EventTypes = PredefinedAssemblyUtil.GetTypes(typeof(IEvent));
            foreach (var VARIABLE in EventTypes)
            {
                Debug.Log(VARIABLE);
            }
            EventBusTypes = InitializeAllBuses();
        }

        static List<Type> InitializeAllBuses()
        {
            var typeDef = typeof(EventBus<>);
            Debug.Log("Initialize all event buses....");
            return EventTypes.Select(eventType => typeDef.MakeGenericType(eventType)).ToList();
        }

        public static void ClearAllBuses()
        {
            Debug.Log("Clearing all event buses....");
            for (var i = 0; i < EventTypes.Count; i++)
            {
                var busType = EventBusTypes[i];
                var clearMethode = busType.GetMethod("Clear",
                    BindingFlags.Static | BindingFlags.Public);
                clearMethode?.Invoke(null, null);
            }
        }
    }
}
