using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace TheMazurkaStudio.Utilities
{
    /// <summary>
    /// Handle services, allow register and get services.
    /// </summary>
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator global;
        private static Dictionary<Scene, ServiceLocator> sceneLocators;
        private static List<GameObject> tmpSceneGameObjects;
        
        private readonly ServiceLocatorManager servicesLocator = new ServiceLocatorManager();

        private const string k_globalServiceLocatorName = "ServiceLocator [GLOBAL]";
        private const string k_sceneServiceLocatorName = "ServiceLocator [SCENE]";

        internal void ConfigureAsGlobal(bool dontDestroyOnLoad)
        {
            if(global == this) Debug.LogWarning("This service locator is already defined as global", this);
            else if(global != null) Debug.LogWarning("Another service locator is already defined as global", this);
            else
            {
                global = this;
                if(dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            }
        }
        internal void ConfigureForScene()
        {
            var scene = gameObject.scene;
            if(sceneLocators.ContainsKey(scene)) Debug.LogError("Another service locator is already defined for this scene", this);
            else
            {
                sceneLocators.Add(scene, this);
            }
        }
        
        
        
        /// <summary>
        /// Retrieve the object service locator, else return scene or global service locator
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        public static ServiceLocator For(MonoBehaviour mb)
        {
            return mb.GetComponentInParent<ServiceLocator>().OrNull() ?? ForSceneOf(mb) ?? Global;
        }
        
        /// <summary>
        /// Retrieve the global service locator
        /// </summary>
        public static ServiceLocator Global
        {
            get
            {
                if (global != null) return global;

                //If anyone exist return
                if (FindFirstObjectByType<ServiceLocatorGlobalBootstrapper>() is { } found)
                {
                    found.BootstrapOnDemand();
                    return global;
                }
                
                //else create one
                var container = new GameObject(k_globalServiceLocatorName, typeof(ServiceLocator));
                container.AddComponent<ServiceLocatorGlobalBootstrapper>().BootstrapOnDemand();
                return global;
            }
        }
        
        /// <summary>
        /// Retrieve the scene level service locator of this gameObject, else return global service locator
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        public static ServiceLocator ForSceneOf(MonoBehaviour mb)
        {
            var scene = mb.gameObject.scene;

            if (sceneLocators.TryGetValue(scene, out var locator) && locator != mb)
            {
                return locator;
            }
            
            //else search in scene
            tmpSceneGameObjects.Clear();
            scene.GetRootGameObjects(tmpSceneGameObjects);
            foreach (var tmpGameObject in tmpSceneGameObjects.Where(tmpGameObject => tmpGameObject.GetComponent<ServiceLocatorSceneBootstrapper>() != null))
            {
                if (tmpGameObject.TryGetComponent(out ServiceLocatorSceneBootstrapper bootstrapper) && bootstrapper.Container != mb)
                {
                    bootstrapper.BootstrapOnDemand();
                    return bootstrapper.Container;
                }
            }

            //else return global
            return global;
        }
        
        
        
        
        /// <summary>
        /// Register a new service of type T to the target service locator
        /// </summary>
        /// <param name="service"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ServiceLocator Register<T>(T service)
        {
            servicesLocator.Register(service);
            return this;
        }
        
        /// <summary>
        /// Register a new service to the target service locator
        /// </summary>
        /// <param name="type"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public ServiceLocator Register(Type type, object service)
        {
            servicesLocator.Register(type, service);
            return this;
        }
        
        /// <summary>
        /// Retrieve the service in the target service locator
        /// </summary>
        /// <param name="service"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public ServiceLocator Get<T>(out T service) where T : class
        {
            if (TryGetService(out service)) return this;

            if (TryGetNextInHierarchy(out var container))
            {
                container.Get(out service);
                return this;
            }

            throw new ArgumentException($"Service of type {typeof(T).FullName} not registered");
        }
        
        /// <summary>
        /// Retrieve the service in the target service locator, return false if service is not found
        /// </summary>
        /// <param name="service"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetService<T>(out T service) where T : class => servicesLocator.TryGet(out service);
        
        /// <summary>
        /// Try get the next service locator in parent hierarchy
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public bool TryGetNextInHierarchy(out ServiceLocator container)
        {
            if (this == global)
            {
                container = null;
                return false;
            }

            container = transform.parent.OrNull()?.GetComponentInParent<ServiceLocator>().OrNull() ?? ForSceneOf(this);
            return container != null;
        }


        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            global = null;
            sceneLocators = new Dictionary<Scene, ServiceLocator>();
            tmpSceneGameObjects = new List<GameObject>();
        }
        private void OnDestroy()
        {
            if (this == global)
            {
                global = null;
            }
            else if (sceneLocators.ContainsValue(this))
            {
                sceneLocators.Remove(gameObject.scene);
            }
        }


#if UNITY_EDITOR
        
        [UnityEditor.MenuItem("GameObject/TheMazurkaStudio/ServiceLocator/Add Global Locator")]
        static void AddGlobal()
        {
            var go = new GameObject(k_globalServiceLocatorName, typeof(ServiceLocatorGlobalBootstrapper));
        }
        
        [UnityEditor.MenuItem("GameObject/TheMazurkaStudio/ServiceLocator/Add Scene Locator")]
        static void AddScene()
        {
            var go = new GameObject(k_sceneServiceLocatorName, typeof(ServiceLocatorSceneBootstrapper));
        }
#endif
    }
}
