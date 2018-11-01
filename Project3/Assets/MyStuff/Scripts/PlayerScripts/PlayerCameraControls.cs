using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControls : MonoBehaviour {

  [Header("Inverts")]
  public bool invertX;
  public bool invertY;

  [Header("Mouse Trackers")]
  float mouseX;
  float mouseY;

  [Header("Rotation Speeds")]
  public float xRotateSpeed;
  public float yRotateSpeed;

  [Header("Y view")]
  [Range(-90, 0)]
  public float minView = -90;
  [Range(0, 90)]
  public float maxView = 90;

  public Transform playerBody;

  void Update ()
  {
    CameraXRotation();
    CameraYRotation();

    if (Input.GetKeyDown(KeyCode.L) && Input.GetKey(KeyCode.LeftShift))
    {
      Cursor.lockState = CursorLockMode.Locked;
    }
	}

  private void CameraXRotation()
  {
    int xInvert;
    if (invertX)
    {
      xInvert = -1;
    }
    else
    {
      xInvert = 1;
    }

    mouseX = Input.GetAxis("Mouse X");

    playerBody.Rotate(Vector3.up, mouseX * xInvert * xRotateSpeed * Time.deltaTime, Space.World);

  }

  private void CameraYRotation()
  {
    float yInvert;

    mouseY = Input.GetAxis("Mouse Y");

    float angelEulerLimit = transform.eulerAngles.x;

    if (angelEulerLimit > 180)
    {
      angelEulerLimit -= 360;
    }

    if (angelEulerLimit < -180)
    {
      angelEulerLimit += 360;
    }

    if (invertY)
    {
      yInvert = -1;
    }
    else
    {
      yInvert = 1;
    }


    float targetRotation = angelEulerLimit + mouseY * yInvert * yRotateSpeed * Time.deltaTime;

    if (targetRotation < maxView && targetRotation > minView)
    {
      transform.eulerAngles += new Vector3(mouseY * yInvert * yRotateSpeed * Time.deltaTime, 0, 0);

    }
  }

}
