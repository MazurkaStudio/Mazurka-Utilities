using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    public class MonoEvents : MonoBehaviour
    {
         [Inject] private Injector injector;
        
        private void Awake()
        {
            Debug.Log("AWAKE " + injector);
            
        }

        private void OnEnable()
        {
            Debug.Log("ENABLE" + injector);
        }

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("START " + injector);
        }
    }
}
