using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{

    public Dictionary<Vector2Int, Tile> grid;
    List<Tile> walkableTiles = new List<Tile>();
    Character character;
    int characterSpeed;

    void Start()
    {
        grid = findAllTiles();

        //-----WARNING-----\\
        //Refactor later
        character = this.transform.GetComponent<Character>();
        //-----WARNING-----\\
    }

    public void findWalkableTiles(Tile startingTile)
    {

        characterSpeed = character.characterClass.speed;

        resetTiles();

        //Use of bfs algorithm
        Queue<Tile> bfsQueue = new Queue<Tile>();

        bfsQueue.Enqueue(startingTile);
        startingTile.searched = true;
        
        while(bfsQueue.Count != 0)
        {
            Tile temp = bfsQueue.Dequeue();
            print("Cost:" +temp.tileType + "Tile Type:" + temp.TileLocation);
            if (temp.calcCost() > characterSpeed)
            {
                continue;
            }

            walkableTiles.Add(temp);

            foreach (Tile neighbor in temp.neighbors)
            {
                if (!neighbor.searched)
                {
                    neighbor.parent = temp;

                    if(neighbor.calcCost() <= characterSpeed)
                    {
                        neighbor.searched = true;

                        bfsQueue.Enqueue(neighbor);
                    }
                }
            }
        }

        HighlightTiles();
    }

    private void resetTiles()
    {
        foreach(var tile in grid)
        {
            tile.Value.resetTile();
        }
    }

    public Dictionary<Vector2Int, Tile> findAllTiles()
    {
        Tile[] allTiles = FindObjectsOfType<Tile>();
        Dictionary<Vector2Int, Tile> tempGrid = new Dictionary<Vector2Int, Tile>();

        foreach (Tile tile in allTiles)
        {
            Vector2Int tilePosition = tile.TileLocation;
            if (tempGrid.ContainsKey(tilePosition))
            {
                Debug.LogError("Overlapping Tiles");
            }
            else
            {
                tempGrid.Add(tilePosition, tile);
            }
        }

        foreach (KeyValuePair<Vector2Int, Tile> tile in tempGrid)
        {
            tile.Value.FindNeighbours(tempGrid);
        }

        return tempGrid;
    }

    public void HighlightTiles()
    {
        foreach(Tile tile in walkableTiles)
        {
            tile.Highlight();
        }
    }
}
