using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// this reloads the scene on certain button clicks
public class MapReseter : MonoBehaviour
{ 
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Backslash))
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
  }
}
