using UnityEngine;

namespace TheMazurkaStudio.Utilities
{
    [ExecuteAlways]
    public class MagnetTransform : MonoBehaviour
    {
        [SerializeField] Vector2 gridSize = Vector2.one;

        public void SetGridSize(Vector2 size) => gridSize = size;
        
        private void OnEnable()
        {
            MagnetToGrid();
            transform.hasChanged = false;
        }

        void Update()
        {
            if (transform.hasChanged)
            {
                MagnetToGrid();
                transform.hasChanged = false;
            }
        }

        public void MagnetToGrid()
        {
            var position = transform.position;
            position.x = Mathf.Round(position.x / gridSize.x) * gridSize.x;
            position.y = Mathf.Round(position.y / gridSize.y) * gridSize.y;
            transform.position = position;
        }
    }
}
