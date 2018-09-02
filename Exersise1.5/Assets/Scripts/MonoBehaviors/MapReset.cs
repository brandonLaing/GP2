using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapReset : MonoBehaviour
{
	void Update ()
  {
    if (Input.GetKeyDown(KeyCode.Backslash))
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
	}
}
