using Enums;
using UnityEngine;

namespace Grid
{
    public class Tile : MonoBehaviour
    {
        public TilesEnum BaseTileType;
        public TilesEnum CurrentTileType;

        private void Start()
        {
            CurrentTileType = BaseTileType;
        }
    }
}