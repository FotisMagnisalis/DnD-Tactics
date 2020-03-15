using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    public Tile currentTile;
    public Tile startingTile;

    public void MoveTo(Tile targetTile)
    {
        //MoveTo
    }

    public static Dictionary<Vector2Int, Tile> findAllTiles()
    {
        Tile[] allTiles = FindObjectsOfType<Tile>();
        Dictionary<Vector2Int, Tile> grid = new Dictionary<Vector2Int, Tile>();

        foreach (Tile tile in allTiles)
        {
            Vector2Int tilePosition = tile.TileLocation;
            if (grid.ContainsKey(tilePosition))
            {
                Debug.LogError("Overlapping Tiles");
            }
            else
            {
                grid.Add(tilePosition, tile);
            }
        }

        foreach(KeyValuePair<Vector2Int,Tile> tile in grid)
        {
            tile.Value.FindNeighbours(grid);
        }

        return grid ;
    }
}
