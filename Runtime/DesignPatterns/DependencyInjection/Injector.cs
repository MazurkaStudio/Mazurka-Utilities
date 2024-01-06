using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Linq;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace TheMazurkaStudio.Utilities
{
    [DefaultExecutionOrder(-1000)]
    public class Injector : Singleton<Injector>, IDependencyProvider
    {
        [SerializeField] private bool injectOnSceneLoaded;
        
        private const BindingFlags k_bindingsFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private readonly Dictionary<Type, object> registry = new Dictionary<Type, object>();

        [Provide] private Injector ProvideInjector() => this;
        
        private void Awake()
        {
            Debug.Log("FIRST INJECTION");

            if (injectOnSceneLoaded)
            {
                InjectScene(gameObject.scene);
                SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            }
            else
            {
                InjectAll();
            }
        }
        protected override void OnDestroySpecific()
        {
            base.OnDestroySpecific();
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        }
        
        
        private void InjectAll()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            //Find all providers (of type IDependencyProvider)
            var providers = FindMonoBehaviors().OfType<IDependencyProvider>();

            foreach (var provider in providers)
            {
                RegisterProvider(provider);
            }

            //Fin all injectables
            var injectables = FindMonoBehaviors().Where(IsInjectable);

            foreach (var injectable in injectables)
            {
                Inject(injectable);
            }
            
            stopwatch.Stop();
            Debug.Log($"Injection time: {stopwatch.ElapsedMilliseconds} milliseconds");
        }
        private void InjectScene(Scene scene)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            //Find all providers (of type IDependencyProvider)
            var providers = FindMonoBehaviorsInScene(scene).OfType<IDependencyProvider>();

            foreach (var provider in providers)
            {
                RegisterProvider(provider);
            }

            //Fin all injectables
            var injectables = FindMonoBehaviorsInScene(scene).Where(IsInjectable);

            foreach (var injectable in injectables)
            {
                Inject(injectable);
            }
            
            stopwatch.Stop();
            Debug.Log($"Injection time: {stopwatch.ElapsedMilliseconds} milliseconds");
        }
        

        private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene == gameObject.scene) return;
            
            Debug.Log("SCENE LOADED " + scene.name);
            InjectScene(scene);
            Debug.Log("ANOTHER INJECTION");
        }

        
        /// <summary>
        /// Inject dependencies on the target instance
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="Exception"></exception>
        public void Inject(object instance)
        {
            var type = instance.GetType();
            var injectableFields = type.GetFields(k_bindingsFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableField in injectableFields)
            {
                var fieldType = injectableField.FieldType;
                var resolvedInstance = Resolve(fieldType);

                if (resolvedInstance == null)
                    throw new Exception($"Failed to inject {fieldType.Name} into {type.Name}");

                injectableField.SetValue(instance, resolvedInstance);
            }
            
            //Fin all injectables fields
            var injectablesMethods = type.GetMethods(k_bindingsFlags).Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectablesMethod in injectablesMethods)
            {
                var requiredParameters = injectablesMethod.GetParameters().Select(parameter => parameter.ParameterType)
                    .ToArray();

                var resolvedInstances = requiredParameters.Select(Resolve).ToArray();

                if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null))
                    throw new Exception($"Failed to inject{type.Name}.{injectablesMethod.Name}");

                injectablesMethod.Invoke(instance, resolvedInstances);
            }
        }

        private object Resolve(Type type)
        {
            registry.TryGetValue(type, out var resolvedInstance);
            return resolvedInstance;
        }

        private static bool IsInjectable(MonoBehaviour obj)
        {
            var members = obj.GetType().GetMembers(k_bindingsFlags);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }

        private void RegisterProvider(IDependencyProvider provider)
        {
            var methods = provider.GetType().GetMethods(k_bindingsFlags);

            foreach (var method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;

                var returnType = method.ReturnType;
                var providedInstance = method.Invoke(provider, null);
                if (providedInstance != null)
                {
                    registry.Add(returnType, providedInstance);
                    Debug.Log($"Registered {returnType.Name} from {provider.GetType().Name}");
                }
                else throw new Exception($"Provider {provider.GetType().Name} returned null for {returnType.Name}");
            }
        }

        private static MonoBehaviour[] FindMonoBehaviors() => FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
        private static IEnumerable<MonoBehaviour> FindMonoBehaviorsInScene(Scene scene) => FindMonoBehaviors().Where(mb => mb.gameObject.scene == scene);
      
    }
}
