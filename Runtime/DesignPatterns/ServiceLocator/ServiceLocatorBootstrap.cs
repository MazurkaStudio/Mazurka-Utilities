using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ServiceLocator))]
    public abstract class ServiceLocatorBootstrap : MonoBehaviour
    {
        private ServiceLocator container;
        internal ServiceLocator Container => container.OrNull() ?? (container = GetComponent<ServiceLocator>());

        private bool hasBeenBootstrapped;

        protected virtual void Awake() => BootstrapOnDemand();
        
        public void BootstrapOnDemand()
        {
            if (hasBeenBootstrapped) return;
            hasBeenBootstrapped = true;
            Bootstrap();
        }

        protected abstract void Bootstrap();
    }
}
