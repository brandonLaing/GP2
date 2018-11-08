using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoaderScript : MonoBehaviour
{
  public LevelChooser chooser;

  private void Start()
  {
    chooser = GameObject.FindGameObjectWithTag("LevelEditorUI").GetComponentInChildren<LevelChooser>();
  }

  public void SendMessageToLevelChooser(Text text)
  {
    chooser.LoadLevel(text);
  }
}
