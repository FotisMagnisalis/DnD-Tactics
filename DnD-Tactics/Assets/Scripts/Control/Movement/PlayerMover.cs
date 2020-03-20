using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : Mover
{

    private void Update()
    {
        Vector3 halfExtens = new Vector3(0, 1, 0);
        Collider[] colliders = Physics.OverlapBox(transform.position, halfExtens);

        foreach(Collider col in colliders)
        {
            if(col.GetComponent<Tile>() != null)
            {
                currentTile = col.GetComponent<Tile>();
                currentTile.currentUser = this;
            }
            
        }
    }

    private void OnMouseDown()
    {
        if(currentTile != null)
        {
            this.transform.GetComponent<Pathfinder>().findWalkableTiles(currentTile);
        }
    }

}
