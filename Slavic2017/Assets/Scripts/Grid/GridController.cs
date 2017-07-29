using UnityEngine;

namespace Grid
{
    public class GridController : MonoBehaviour
    {
        public int[,] Tiles;

        public int GridColumnsCount;
        public int GridRowsCount;

        // Use this for initialization
        void Start ()
        {
            GridColumnsCount = transform.childCount;
            GridRowsCount = transform.GetChild(0).childCount;

            Tiles = new int[GridColumnsCount, GridRowsCount];

            for (int i = 0; i < GridColumnsCount; i++)
            {
                for (int j = 0; j < GridRowsCount; j++)
                {
                    GameObject tile = transform.GetChild(i).GetChild(j).gameObject;	          
                    Tiles[i,j] = (int) tile.GetComponent<Tile>().TileType;
                }
            }	
        }
	
        // Update is called once per frame
        void Update () {
		
        }
    }
}
