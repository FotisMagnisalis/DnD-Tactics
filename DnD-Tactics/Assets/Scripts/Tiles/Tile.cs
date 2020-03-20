using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour
{
    //Pathfinding info
    public Vector2Int TileLocation;
    public TileType tileType;
    public int cost;
    public bool hovered = false;
    public List<Tile> neighbors;
    public Tile parent;
    public Mover currentUser;
    public bool searched;

    //UI information
    [SerializeField] Material baseMaterial;
    [SerializeField] Material mouseOverMaterial;


    private void OnMouseOver()
    {
        if (!hovered && tileType != TileType.impossible)
        {
            hovered = true;
            transform.GetChild(0).GetComponent<MeshRenderer>().material = mouseOverMaterial;
        }
    }

    private void OnMouseExit()
    {
        if (hovered)
        {
            hovered = false;
            transform.GetChild(0).GetComponent<MeshRenderer>().material = baseMaterial;
        }
    }

    public void Highlight()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material = mouseOverMaterial;
    }

    public void InitLocation()
    {
        TileLocation.x = (int)transform.position.x;
        TileLocation.y = (int)transform.position.z; //We don't care about the y in this context

        EditorSceneManager.MarkSceneDirty(this.gameObject.scene);
        EditorUtility.SetDirty(this);
    }

    public void FindNeighbours(Dictionary<Vector2Int, Tile> grid)
    {
        if(this.neighbors.Count == 0)
        {
            neighbors = new List<Tile>();

            Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
            for(int i = 0; i < directions.Length; i++)
            {
                insertNeighbour(directions[i], grid);
            }
        }
    }

    private void insertNeighbour(Vector2Int vectorDirection, Dictionary<Vector2Int, Tile> grid)
    {
        var neighbourTile = TileLocation + vectorDirection;

        if (grid.ContainsKey(neighbourTile))
        {
            neighbors.Add(grid[neighbourTile]);
        }
    }

    public bool checkIfOccupied()
    {
        return currentUser != null;
    }

    public int calcCost()
    {
        if(this.parent != null)
        {
            return (int) tileType + parent.calcCost();
        }
        else
        {
            return (int) tileType;
        }
    }

    //Resets tile's pathfinder attributes
    public void resetTile()
    {
        parent = null;
        searched = false;
    }
}
