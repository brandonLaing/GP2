using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this holds the level name and its appearence in the scene, when player hits any load save or new button it goes though this then the save load level editor
public class LevelChooser : MonoBehaviour
{
  public InputField levelName;
  public SaveLoadLevel saveLoadManager;

  public GameObject levelLoader;
  public GameObject levelRenamer;
  public GameObject levelCreator;

  public GameObject loadLevelPrefab;

  public string[] listOfSaves;
  public List<GameObject> currentSaves = new List<GameObject>();

  public void OpenLevelLoader(bool open)
  {
    levelLoader.SetActive(open);

    if (open && SaveLoadSystem.GetAllLevels().Length > 0)
    {
      foreach (GameObject saveButton in currentSaves)
      {
        Destroy(saveButton);
      }

      currentSaves = new List<GameObject>();

      listOfSaves = SaveLoadSystem.GetAllLevels();

      for (int i = 0; i < listOfSaves.Length; i++)
      {
        GameObject newButton = Instantiate(loadLevelPrefab, levelLoader.transform);
        newButton.GetComponentInChildren<Text>().text = listOfSaves[i];
        currentSaves.Add(newButton);
      }
    }
  }

  public void BackToMenu()
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
  }

  public void OpenLevelCreator(bool open)
  {
    levelCreator.SetActive(open);
  }

  public void CreateLevel(InputField levelName)
  {
    if (levelName.text != string.Empty)
    {
      SaveLoadSystem.MakeNewLevelSave(levelName.text);
      saveLoadManager.LoadLevel(levelName.text);
      OpenLevelCreator(false);
    }
  }

  public void LoadLevel(Text levelName)
  {
    OpenLevelLoader(false);
    saveLoadManager.LoadLevel(levelName.text);
  }

  public void LevelLoaded(string levelName)
  {
    this.levelName.text = levelName;
  }

  public void SaveLevel()
  {
    if (levelName.text != string.Empty)
    {
      saveLoadManager.SaveLevel(levelName.text);
    }
  }
}
