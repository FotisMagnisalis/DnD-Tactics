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

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(tiles[0].scene);

        //Assign the position
        foreach (GameObject t in tiles)
        {
            t.GetComponent<Tile>().InitLocation();
        }
    }
}
