using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLevelChooser : MonoBehaviour
{
  public List<GameObject> levelUIDisplayers = new List<GameObject>();
  public GameObject chooserPrefab;
  public Transform chooserHolder;

  private void OnEnable()
  {
    SaveLoadSystem.MakeDirectory();

    foreach (GameObject obj in levelUIDisplayers)
    {
      Destroy(obj);
    }

    foreach (string save in Directory.GetFiles(SaveLoadSystem.SavesDirectory))
    {
      string saveName = save
                            .Replace(SaveLoadSystem.SavesDirectory, "")
                            .Replace("_Save.map", "");

      GameObject newChooser = Instantiate(chooserPrefab, chooserHolder);
      newChooser.GetComponentInChildren<Text>().text = saveName;
    }
  }
}
