using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public TileType[] TileTypes;
    private int[,] tiles;
    private int mapSizeX = 10;
    private int mapSizeY = 10;

    void Start()
    {
        tiles = new int[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeX; y++)
            {
                tiles[x, y] = 0;
            }
        }
        ConvertToRock();
        GenerateMapVisual();
    }

    void GenerateMapVisual()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeX; y++)
            {
                TileType tileType = TileTypes[tiles[x, y]];
                Instantiate(tileType.tileVisualPrefab, new Vector3(x, 0, y), Quaternion.identity);
            }
        }

    }

    void ConvertToRock()
    {
        //temp
        tiles[4, 4] = 2;
        tiles[5, 4] = 2;
        tiles[6, 4] = 2;
        tiles[7, 4] = 2;
        tiles[8, 4] = 2;

        tiles[4, 5] = 2;
        tiles[4, 6] = 2;
        tiles[8, 5] = 2;
        tiles[8, 6] = 2;
    }
}
