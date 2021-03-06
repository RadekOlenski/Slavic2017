﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Enums;

namespace Grid
{
    public class TileMap : MonoBehaviour
    {
        public int MapSizeX;
        public int MapSizeY;

        public GridController GridController;

        public GameObject SelectedUnit;

        public TileType[] tileTypes;

        int[,] tiles;
        Node[,] graph;

        private List<GameObject> instantiatedGrid;

        private void Awake()
        {
            instantiatedGrid = new List<GameObject>();
        }

        private void Start()
        {
            MapSizeX = GridController.GridColumnsCount;
            MapSizeY = GridController.GridRowsCount;
            tiles = GridController.TileTypes;

            SetupSelectedUnit();

            GeneratePathfindingGraph();
            GenerateMapVisual();
        }

        public void SetupSelectedUnit()
        {
            // Setup the SelectedUnit's variable
            //SelectedUnit.GetComponent<Unit>().currentPath = null;
            SelectedUnit.GetComponent<Unit>().tileX = Convert.ToInt32(SelectedUnit.transform.position.x);
            SelectedUnit.GetComponent<Unit>().tileY = Convert.ToInt32(SelectedUnit.transform.position.z);
            SelectedUnit.GetComponent<Unit>().map = this;
        }

        void OnDisable()
        {
            foreach (GameObject gameObject in instantiatedGrid)
            {
                Destroy(gameObject);
            }
        }

        public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
        {
            TileType tt = tileTypes[tiles[targetX, targetY]];

            if (UnitCanEnterTile(targetX, targetY) == false)
                return Mathf.Infinity;

            float cost = tt.movementCost;

            if (sourceX != targetX && sourceY != targetY)
            {
                // We are moving diagonally!  Fudge the cost for tie-breaking
                // Purely a cosmetic thing!
                cost += 0.001f;
            }
            return cost;
        }

        void GeneratePathfindingGraph()
        {
            // Initialize the array
            graph = new Node[MapSizeX, MapSizeY];

            // Initialize a Node for each spot in the array
            for (int x = 0; x < MapSizeX; x++)
            {
                for (int y = 0; y < MapSizeY; y++)
                {
                    graph[x, y] = new Node();
                    graph[x, y].x = x;
                    graph[x, y].y = y;
                }
            }

            // Now that all the nodes exist, calculate their neighbours
            for (int x = 0; x < MapSizeX; x++)
            {
                for (int y = 0; y < MapSizeY; y++)
                {
                    // This is the 4-way connection version:
                    /*				if(x > 0)
                                    graph[x,y].neighbours.Add( graph[x-1, y] );
                                if(x < MapSizeX-1)
                                    graph[x,y].neighbours.Add( graph[x+1, y] );
                                if(y > 0)
                                    graph[x,y].neighbours.Add( graph[x, y-1] );
                                if(y < MapSizeY-1)
                                    graph[x,y].neighbours.Add( graph[x, y+1] );
                */

                    // This is the 8-way connection version (allows diagonal movement)
                    // Try left
                    if (x > 0)
                    {
                        graph[x, y].neighbours.Add(graph[x - 1, y]);
                        if (y > 0)
                            graph[x, y].neighbours.Add(graph[x - 1, y - 1]);
                        if (y < MapSizeY - 1)
                            graph[x, y].neighbours.Add(graph[x - 1, y + 1]);
                    }

                    // Try Right
                    if (x < MapSizeX - 1)
                    {
                        graph[x, y].neighbours.Add(graph[x + 1, y]);
                        if (y > 0)
                            graph[x, y].neighbours.Add(graph[x + 1, y - 1]);
                        if (y < MapSizeY - 1)
                            graph[x, y].neighbours.Add(graph[x + 1, y + 1]);
                    }

                    // Try straight up and down
                    if (y > 0)
                        graph[x, y].neighbours.Add(graph[x, y - 1]);
                    if (y < MapSizeY - 1)
                        graph[x, y].neighbours.Add(graph[x, y + 1]);

                    // This also works with 6-way hexes and n-way variable areas (like EU4)
                }
            }
        }

        void GenerateMapVisual()
        {
            for (int x = 0; x < MapSizeX; x++)
            {
                for (int y = 0; y < MapSizeY; y++)
                {
                    TileType tt = tileTypes[tiles[x, y]];
                    GameObject go =
                        (GameObject) Instantiate(tt.tileVisualPrefab, new Vector3(x, 0, y), Quaternion.identity);
                    instantiatedGrid.Add(go);

                    ClickableTile ct = go.GetComponent<ClickableTile>();
                    ct.tileX = x;
                    ct.tileY = y;
                    ct.map = this;
                }
            }
        }

        public Vector3 TileCoordToWorldCoord(int x, int y)
        {
            return new Vector3(x, 0, y);
        }

        public bool UnitCanEnterTile(int x, int y)
        {
            // We could test the unit's walk/hover/fly type against various
            // terrain flags here to see if they are allowed to enter the tile.

            tiles = GridController.TileTypes;
            if (SelectedUnit.CompareTag(TagsEnum.Enemy))
            {
                if (!tileTypes[tiles[x, y]].isWalkable || tileTypes[tiles[x, y]].BlockMonsters)
                {
                    return false;
                }
            }

            return tileTypes[tiles[x, y]].isWalkable;
        }

        public void GeneratePathTo(int x, int y)
        {
            // Clear out our unit's old path.
            SelectedUnit.GetComponent<Unit>().currentPath = null;

            if (SelectedUnit.CompareTag(TagsEnum.Enemy))
            {
                TileType tt = null;
                for (int i = 0; i < GridController.GridColumnsCount; i++)
                {
                    for (int j = 0; j < GridController.GridRowsCount; j++)
                    {
                        GameObject tile = GridController.transform.GetChild(i).GetChild(j).gameObject;
                        if (Vector3.Distance(SelectedUnit.transform.position, tile.transform.position) < 0.1f)
                        {
                            tt = tileTypes[tiles[i, j]];
                            break;
                        }
                    }
                }

                if (tt != null)
                {
                    if (tt.BlockMonsters)
                    {
                        SelectedUnit.GetComponent<Unit>().ActionPoints = 0;
                        return;
                    }

                }
            }

            if (UnitCanEnterTile(x, y) == false)
            {
                // We probably clicked on a mountain or something, so just quit out.
                return;
            }

            Dictionary<Node, float> dist = new Dictionary<Node, float>();
            Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

            // Setup the "Q" -- the list of nodes we haven't checked yet.
            List<Node> unvisited = new List<Node>();

            Node source = graph[SelectedUnit.GetComponent<Unit>().tileX, SelectedUnit.GetComponent<Unit>().tileY];

            Node target = graph[x, y];

            dist[source] = 0;
            prev[source] = null;

            // Initialize everything to have INFINITY distance, since
            // we don't know any better right now. Also, it's possible
            // that some nodes CAN'T be reached from the source,
            // which would make INFINITY a reasonable value
            foreach (Node v in graph)
            {
                if (v != source)
                {
                    dist[v] = Mathf.Infinity;
                    prev[v] = null;
                }

                unvisited.Add(v);
            }

            while (unvisited.Count > 0)
            {
                // "u" is going to be the unvisited node with the smallest distance.
                Node u = null;

                foreach (Node possibleU in unvisited)
                {
                    if (u == null || dist[possibleU] < dist[u])
                    {
                        u = possibleU;
                    }
                }

                if (u == target)
                {
                    break; // Exit the while loop!
                }

                unvisited.Remove(u);

                foreach (Node v in u.neighbours)
                {
                    //float alt = dist[u] + u.DistanceTo(v);
                    float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v] = u;
                    }
                }
            }

            // If we get there, the either we found the shortest route
            // to our target, or there is no route at ALL to our target.

            if (prev[target] == null)
            {
                // No route between our target and the source
                if (SelectedUnit.CompareTag(TagsEnum.Enemy))
                {
                    SelectedUnit.GetComponent<Unit>().ActionPoints = 0;
                }
                return;
            }

            List<Node> currentPath = new List<Node>();

            Node curr = target;

            // Step through the "prev" chain and add it to our path
            while (curr != null)
            {
                currentPath.Add(curr);
                curr = prev[curr];
            }

            // Right now, currentPath describes a route from out target to our source
            // So we need to invert it!

            currentPath.Reverse();

            var unit = SelectedUnit.GetComponent<Unit>();
            if (unit.gameObject.CompareTag(TagsEnum.Enemy))
            {
                unit.currentPath = currentPath;
                return;
            }
            unit.currentPath = unit.ActionPoints + .1f < CalculateTotalPathCost(currentPath) ? null : currentPath;
        }

        private float CalculateTotalPathCost(List<Node> currentPath)
        {
            float totalCost = 0;
            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                totalCost += CostToEnterTile(currentPath[i].x, currentPath[i].y, currentPath[i + 1].x,
                    currentPath[i + 1].y);
            }
            return totalCost;
        }
    }
}