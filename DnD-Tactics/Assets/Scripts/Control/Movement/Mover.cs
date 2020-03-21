using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour
{
    public Tile currentTile;
    Stack<Tile> path = new Stack<Tile>();
    public Pathfinder pathfinder;
    public bool moving = false;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    public Character characterClass;

    public bool isActive = false;

    public void StartMovement(Tile targetTile)
    {
        if(path != null)
        {
            path.Clear();
        }

        path.Push(targetTile);

        FindParents(targetTile);

        moving = true;
    }

    private void FindParents(Tile targetTile)
    {
        if(targetTile.parent != null)
        {
            path.Push(targetTile.parent);
            FindParents(targetTile.parent);
        }
    }

     public void Move()
    {
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            if(Vector3.Distance(transform.position,target) >= 0.05f)
            {
                CalculateHeading(target);
                SetHorizontalVelocity();

                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            moving = false;

            pathfinder.resetTiles();

            path.Clear();

            //TO BE REMOVED
            isActive = false;
            //
        }
    }

    private void SetHorizontalVelocity()
    {
        velocity = heading * characterClass.characterClass.speed;
    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }
    
    public void FindCurrentBlock()
    {
        Vector3 halfExtens = new Vector3(0, 1, 0);
        Collider[] colliders = Physics.OverlapBox(transform.position, halfExtens);

        foreach (Collider col in colliders)
        {
            if (col.GetComponent<Tile>() != null)
            {
                currentTile = col.GetComponent<Tile>();
                currentTile.currentUser = this;
            }

        }
    }
}
