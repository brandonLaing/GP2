using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
  public GameObject levelChooser;

  public void OpenCloseLevelChooser(bool onOff)
  {
    levelChooser.SetActive(onOff);
  }

  public void LoadLevel(Text levelName)
  {

  }

  public void GoToLevelEditor()
  {
    SceneManager.LoadScene("EditorScene");
  }

  public void QuitGame()
  {
    Application.Quit();
  }
}
