using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    [AddComponentMenu("TheMazurkaStudio/ServiceLocator/SceneServiceLocator")]
    public class ServiceLocatorSceneBootstrapper : ServiceLocatorBootstrap
    {
        protected override void Bootstrap()
        {
            Container.ConfigureForScene();
        }
    }
}