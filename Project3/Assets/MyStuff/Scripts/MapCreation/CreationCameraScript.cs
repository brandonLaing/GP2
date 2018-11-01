using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationCameraScript : MonoBehaviour
{
  public float cameraMoveSpeed;
  public float xRotationSpeed = 100;
  public float yRotationSpeed = 100;

  [Range(-90, 0)]
  public float yMinView = -90;
  [Range(0, 90)]
  public float yMaxView = 90;

  public bool invertX = false;
  public bool invertY = true;

  private float mouseX;
  private float mouseY;

  private void Start()
  {
    Cursor.lockState = CursorLockMode.Confined;
  }

  private void Update()
  {
    if (Input.GetMouseButton(1))
    {
      MouseControls();
      WASDControls();
    }

    if (Cursor.lockState != CursorLockMode.Confined && Input.anyKey)
    {
      Cursor.lockState = CursorLockMode.Confined;
    }
  }

  // camera rotation rotation controls
  private void MouseControls()
  {
    #region X Rotation
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

    transform.Rotate(Vector3.up, mouseX * xInvert * xRotationSpeed * Time.deltaTime, Space.World);
    #endregion

    #region Y Rotation
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


    float targetRotation = angelEulerLimit + mouseY * yInvert * yRotationSpeed * Time.deltaTime;

    if (targetRotation < yMaxView && targetRotation > yMinView)
    {
      transform.eulerAngles += new Vector3(mouseY * yInvert * yRotationSpeed * Time.deltaTime, 0, 0);

    }
    #endregion
  }

  // camera position controls
  private void WASDControls()
  {
    Vector3 moveDirection = new Vector3();

    // forward
    if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftShift))
      moveDirection += transform.forward;

    // back
    if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.LeftShift))
      moveDirection -= transform.forward;

    // up
    if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
      moveDirection += transform.up;

    // down
    if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
      moveDirection -= transform.up;

    // right
    if (Input.GetKey(KeyCode.D))
      moveDirection += transform.right;

    // left
    if (Input.GetKey(KeyCode.A))
      moveDirection -= transform.right;

    transform.position += moveDirection.normalized * cameraMoveSpeed * Time.deltaTime;

  }

  public void Focus(GameObject obj)
  {
    transform.position = new Vector3(0, 5, -5);
    transform.LookAt(obj.transform);

  }
}
