using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileAssigner : MonoBehaviour
{
    [MenuItem("Tools/Assign Tile Location")]
    public static void AssignTileLocation()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");


        //Assign the position
        foreach(GameObject t in tiles)
        {

            t.GetComponent<Tile>().TileLocation.x = (int) t.transform.position.x;
            t.GetComponent<Tile>().TileLocation.y = (int) t.transform.position.z; //We don't care about the y in this context
        }
    }
}
