using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapReset : MonoBehaviour
{
  public static TileGenerator tileGen;

	void Update ()
  {
    // re loads the scene
    if (Input.GetKeyDown(KeyCode.Backslash) && !Input.GetKey(KeyCode.LeftShift))
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    // rechecks connections for all tiles
    if (Input.GetKeyDown(KeyCode.Backslash) && Input.GetKey(KeyCode.LeftShift))
    {
      tileGen.ReCheckConnections();

    }
  }
}
