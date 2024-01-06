using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    [AddComponentMenu("TheMazurkaStudio/ServiceLocator/GlobalServiceLocator")]
    public class ServiceLocatorGlobalBootstrapper : ServiceLocatorBootstrap
    {
        [SerializeField] private bool dontDestroyOnLoad = true;
        
        protected override void Bootstrap()
        {
            Container.ConfigureAsGlobal(dontDestroyOnLoad);
        }
    }
}