using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(FireControlsScript))]
public class PlayerController : NetworkBehaviour
{
  public Camera playerCamera;

  private float mouseX;
  private float mouseXRotationSpeed = 100;
  private float mouseY;
  private float mouseYRotationSpeed = 100;

  private float maxView = 90.0F;
  private float minView = -90.0F;

  private FireControlsScript fireController;

  private void Start()
  {
    fireController = GetComponent<FireControlsScript>();

    if (!CheckIfIsLocalPlayer())
    {
      this.enabled = false;
      fireController.enabled = false;
    }
  }
  public float speed = 10f;
  public override void OnStartLocalPlayer()
  {
    GetComponent<MeshRenderer>().material.color = Color.blue;
    playerCamera.enabled = true;
  }

  void Update()
  {
    MovementControls();
    CameraControls();

  }

  private void MovementControls()
  {
    Vector3 movementDirection = new Vector3();
    if (Input.GetKey(KeyCode.W)) { movementDirection += transform.forward; }
    if (Input.GetKey(KeyCode.A)) { movementDirection += -transform.right; }
    if (Input.GetKey(KeyCode.S)) { movementDirection += -transform.forward; }
    if (Input.GetKey(KeyCode.D)) { movementDirection += transform.right; }

    transform.position += movementDirection.normalized * speed * Time.deltaTime;
  }

  private void CameraControls()
  {
    mouseX = Input.GetAxis("Mouse X");
    transform.Rotate(Vector3.up, mouseX * mouseXRotationSpeed * Time.deltaTime);

    mouseY = Input.GetAxis("Mouse Y");

    float angleEulerLimit = playerCamera.transform.eulerAngles.x;

    if (angleEulerLimit > 180)
    {
      angleEulerLimit -= 360;
    }

    if (angleEulerLimit < -180)
    {
      angleEulerLimit += 360;
    }

    float targetRotation = angleEulerLimit + mouseY * -mouseYRotationSpeed * Time.deltaTime;

    if (targetRotation < maxView && targetRotation > minView)
    {
      playerCamera.transform.eulerAngles += new Vector3(mouseY * -mouseYRotationSpeed * Time.deltaTime, 0, 0);

    } // end TARGET ROTATION

  }

  private bool CheckIfIsLocalPlayer()
  {
    if (isLocalPlayer)
    {
      return true;
    }

    return false;
  }
}
