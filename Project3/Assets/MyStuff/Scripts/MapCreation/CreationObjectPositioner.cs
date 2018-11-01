using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Axiss
{
  xAxis,
  yAxis,
  zAxis
}

public class CreationObjectPositioner : MonoBehaviour
{
  public Transform target;
  public Vector3 temps;

  public Transform xAxis;
  public Transform yAxis;
  public Transform zAxis;

  public float transformSpeed = 5;

  public bool lastTryHit;

  public Axiss currentAxis;

  private Vector3 startMousePosition = new Vector3();
  public Vector3 currentMousePosition = new Vector3();
  private void Update()
  {
    MoveObject();


    UpdateTargetLocation();
  }

  private void MoveObject()
  {
    // check if we hit something we need to
    if (Input.GetMouseButtonDown(0))
    {
      startMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
      //RaycastHit hit;
      //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      //if (Physics.Raycast(ray, out hit))
      //{
      //  lastTryHit = false;

      //  Debug.Log(hit.transform.name);

      //  if (hit.transform == xAxis)
      //  {
      //    Debug.Log("Hit X");
      //    currentAxis = Axiss.xAxis;
      //    lastTryHit = true;
      //    previousMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
      //    Cursor.lockState = CursorLockMode.Locked;
      //    return;
      //  }

      //  if (hit.transform == yAxis)
      //  {
      //    currentAxis = Axiss.yAxis;
      //    lastTryHit = true;
      //    previousMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
      //    Cursor.lockState = CursorLockMode.Locked;
      //    return;
      //  }

      //  if (hit.transform == zAxis)
      //  {
      //    currentAxis = Axiss.zAxis;
      //    lastTryHit = true;
      //    previousMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
      //    Cursor.lockState = CursorLockMode.Locked;
      //    return;
      //  }
      //}
    }

    if (Input.GetKey(KeyCode.Z))
      currentAxis = Axiss.xAxis;

    if (Input.GetKey(KeyCode.X))
      currentAxis = Axiss.yAxis;

    if (Input.GetKey(KeyCode.C))
      currentAxis = Axiss.zAxis;


    if (Input.GetMouseButton(0))
    {
      if (currentAxis == Axiss.xAxis)
      {
        Debug.Log("Moving X");
        currentMousePosition = (startMousePosition - Camera.main.ScreenToViewportPoint(Input.mousePosition));
        currentMousePosition.z = 0;
        currentMousePosition.y = 0;
        if (currentMousePosition.x > .2F || currentMousePosition.x < -0.2F)
        {
          transform.position += currentMousePosition * -1 * transformSpeed * Time.deltaTime;

        }
      }

      if (currentAxis == Axiss.yAxis)
      {
        Debug.Log("Moving X");
        currentMousePosition = (startMousePosition - Camera.main.ScreenToViewportPoint(Input.mousePosition));
        currentMousePosition.z = 0;
        currentMousePosition.x = 0;
        if (currentMousePosition.y > .2F || currentMousePosition.y < -0.2F)
        {
          transform.position += currentMousePosition * -1 * transformSpeed * Time.deltaTime;

        }
      }

      if (currentAxis == Axiss.zAxis)
      {
        Debug.Log("Moving X");
        currentMousePosition = (startMousePosition - Camera.main.ScreenToViewportPoint(Input.mousePosition));
        currentMousePosition.y = 0;
        currentMousePosition.z = currentMousePosition.x;
        currentMousePosition.x = 0;
        if (currentMousePosition.z > .1F || currentMousePosition.z < -0.1F)
        {
          transform.position += currentMousePosition * -1 * transformSpeed * Time.deltaTime;

        }
      }
    }
  }

  private void UpdateTargetLocation()
  {
    if (target != null)
    {
      target.position = this.transform.position;
    }
  }


  public void SetNewTarget(Transform target)
  {
    transform.position = target.position;
    this.target = target;
  }
}
