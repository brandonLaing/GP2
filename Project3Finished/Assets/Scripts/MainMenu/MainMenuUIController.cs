using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
  public void LoadEditor()
  {
    SceneManager.LoadScene("EditorScene");
  }

  public void LoadLevelPlayer()
  {
    try
    {
      SceneManager.LoadScene("GameScene");
      Debug.Log("Game scene loaded successfully");
    }
    catch (Exception ex)
    {
      Debug.Log(string.Format("Exception thrown loading Game Scene: {0}\r\n{1}", ex.Message, ex.StackTrace));
    }
  }

  public void QuitGame()
  {
    Application.Quit();
  }
}
