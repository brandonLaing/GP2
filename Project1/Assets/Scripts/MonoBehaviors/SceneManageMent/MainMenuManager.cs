using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Application.Quit();

    }
  }
  public void _LoadNewScene(string scene)
  {
    SceneManager.LoadScene(scene);

  }

  public void _CloseGame()
  {
    Application.Quit();

  }
}
