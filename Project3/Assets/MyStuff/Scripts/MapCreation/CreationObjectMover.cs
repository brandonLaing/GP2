using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreationObjectMover : MonoBehaviour
{
  public GameObject target;
  public Camera myCam;
  public CreationObjectPositioner positioner;
  public Text nameText;

  private void Update()
  {
    CheckForNewTarget();
    CheckToFocus();

  }

  private void CheckToFocus()
  {
    if (Input.GetKeyDown(KeyCode.F) && target != null)
      myCam.GetComponent<CreationCameraScript>().Focus(target);
  }

  private void CheckForNewTarget()
  {
    if (Input.GetMouseButton(0))
    {
      RaycastHit hit;
      Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out hit))
      {
        Transform objectHit = hit.transform;

        if (objectHit.tag == "Props")
        {
          target = objectHit.gameObject;
          positioner.target = objectHit;
        }
      }
    }
  }

}
