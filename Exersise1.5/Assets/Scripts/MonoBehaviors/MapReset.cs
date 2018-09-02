using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapReset : MonoBehaviour
{
  public static TileGenerator tileGen;

  private float checkCounter = 0;

	void Update ()
  {
    if (Input.GetKeyDown(KeyCode.Backslash) && !Input.GetKey(KeyCode.LeftShift))
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    if (Input.GetKeyDown(KeyCode.Backslash) && Input.GetKey(KeyCode.LeftShift))
    {
      tileGen.ReCheckConnections();

    }
  }
}
