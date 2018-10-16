using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCameraController : MonoBehaviour
{
  public Camera cam;
  public GameObject focusedPlayer;
  bool startingCountdown;

  private void Update()
  {
    if (focusedPlayer != null)
    {
      transform.position = focusedPlayer.gameObject.transform.position + new Vector3(0, 1, 2);
      transform.LookAt(focusedPlayer.transform);
      transform.Rotate(Vector3.up, 5);

      if (!startingCountdown)
      {
        StartCoroutine(StartEndCountdown());
        startingCountdown = !startingCountdown;
      }
    }
  }

  IEnumerator StartEndCountdown()
  {
    yield return new WaitForSeconds(5);
    GameObject.FindGameObjectWithTag("NetworkController").GetComponent<CustomNetworkControl>().ClientDisconnect();
  }

  public void Showcase(GameObject player)
  {
    cam.enabled = true;
    focusedPlayer = player.gameObject;



  }
}
