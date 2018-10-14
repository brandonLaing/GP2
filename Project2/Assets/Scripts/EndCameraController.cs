using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCameraController : MonoBehaviour
{
  public Camera cam;
  public GameObject focusedPlayer;

  private void Update()
  {
    if (focusedPlayer != null)
    {
      transform.position = focusedPlayer.gameObject.transform.position + new Vector3(0, 1, 2);
      transform.LookAt(focusedPlayer.transform);
      transform.Rotate(Vector3.up, 5);

    }
  }

  public void Showcase(GameObject player)
  {
    cam.enabled = true;
    focusedPlayer = player.gameObject;



  }
}
