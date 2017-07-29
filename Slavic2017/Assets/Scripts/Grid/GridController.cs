using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridController : MonoBehaviour
    {
        public int[,] TileTypes;
        public List<Tile> Tiles;

        public int GridColumnsCount;
        public int GridRowsCount;

        // Use this for initialization
        void Start ()
        {

        }
	
        // Update is called once per frame
        void Update () {
            GridColumnsCount = transform.childCount;
            GridRowsCount = transform.GetChild(0).childCount;

            TileTypes = new int[GridColumnsCount, GridRowsCount];
            Tiles = new List<Tile>();

            for (int i = 0; i < GridColumnsCount; i++)
            {
                for (int j = 0; j < GridRowsCount; j++)
                {
                    GameObject tile = transform.GetChild(i).GetChild(j).gameObject;
                    TileTypes[i, j] = (int)tile.GetComponent<Tile>().CurrentTileType;
                    Tiles.Add(tile.GetComponent<Tile>());
                }
            }
        }
    }
}
