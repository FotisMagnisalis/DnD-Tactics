using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : Mover
{
    public Dictionary<Vector2Int,Tile> grid;

    private void Start()
    {
        grid = findAllTiles();
    }

}
