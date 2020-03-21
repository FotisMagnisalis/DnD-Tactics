using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : Mover
{

    private void Update()
    {
        FindCurrentBlock();

        if (moving)
        {
            Move();
            return;
        }


        //If is not moving yet, start moving
        if (isActive)
        {
            processAction();
        }
    }

    private void processAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
            foreach(RaycastHit hit in hits)
            {
                Tile targetTile = hit.transform.GetComponent<Tile>();

                if (targetTile == null) continue;

                if (pathfinder.walkableTiles.Contains(targetTile))
                {
                    StartMovement(targetTile);
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if(currentTile != null)
        {
            pathfinder.findWalkableTiles(currentTile);

            pathfinder.HighlightTiles();

            StartCoroutine(setActive());
        }
    }

    IEnumerator setActive()
    {
        yield return new WaitForSeconds(1);

        isActive = true;
    }

}
