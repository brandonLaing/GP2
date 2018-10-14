using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

  public static bool endGameCheck;
  public EndCameraController endCamera;

  public void EndGame(GameObject player)
  {
    if (!endGameCheck)
    {
      DoEndGame(player);

    }
  }

  private void DoEndGame(GameObject player)
  {
    foreach (GameObject score in GameObject.FindGameObjectsWithTag("Player"))
    {
      score.GetComponent<PlayerScore>().enabled = false;
      score.GetComponentInChildren<Camera>().enabled = false;

      endCamera.Showcase(player);


    }
  }
}
