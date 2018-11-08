using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelChooserScript : MonoBehaviour
{
  public GameSceneController sceneController;


  public void SendMessageToController(Text text)
  {
    sceneController.LoadLevel(text.text);
  }
}
