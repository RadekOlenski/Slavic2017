using Grid;
using UnityEngine;

namespace Interactions
{
    public class MovementModeController : MonoBehaviour
    {
        public TileMap TileMap;

        // Use this for initialization
        void Start()
        {
            Debug.Log("Movement mode enabled");
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnEnable()
        {
            ClickableTile.IsEnabled = true;
        }

        private void OnDisable()
        {
            //TileMap.enabled = false;
            ClickableTile.IsEnabled = false;
        }
    }
}