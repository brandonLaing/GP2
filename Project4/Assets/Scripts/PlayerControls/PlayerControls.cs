using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlMode
{
  Build, Unit
}

public class PlayerControls : MonoBehaviour
{
  public ControlMode currentMode;

  public LayerMask unitLayer;
  public List<UnitController> selectedUnits;

  public LayerMask tileLayer;
  public LayerMask buildingLayer;
  public PathingTile selectedTile;

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.B))
    {
      currentMode = ControlMode.Build;
    }
    if (Input.GetKeyDown(KeyCode.V))
    {
      currentMode = ControlMode.Unit;
    }

    if (currentMode == ControlMode.Build)
    {
      if (Input.GetMouseButtonDown(1))
      {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, tileLayer))
        {
          selectedTile = hit.transform.GetComponent<PathingTile>();

          selectedTile.BuildUnitSpawner();
        }
      }
      if (Input.GetMouseButtonDown(0))
      {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, buildingLayer))
        {
          if (hit.transform.GetComponentInParent<UnitSpawnerController>() != null)
          {
            var script = hit.transform.GetComponentInParent<UnitSpawnerController>();
            script.BuildNewUnit();
          }
        }
      }
    }

    if (currentMode == ControlMode.Unit)
    {
      if (Input.GetMouseButtonDown(0))
      {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, unitLayer))
        {
          if (!Input.GetKey(KeyCode.LeftShift))
          {
            selectedUnits = new List<UnitController>()
            {
              hit.transform.gameObject.GetComponent<UnitController>()
          };
          }
          else
          {
            selectedUnits.Add(hit.transform.gameObject.GetComponent<UnitController>());
          }
        }
      }

      if (Input.GetMouseButtonDown(1))
      {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, tileLayer))
        {
          if (!Input.GetKey(KeyCode.LeftShift))
          {
            foreach (UnitController unit in selectedUnits)
            {
              unit.SetNewPoint(hit.transform.GetComponent<PathingTile>().tileNode);
            }
          }
          else
          {
            foreach (UnitController unit in selectedUnits)
              unit.AddNewPoint(hit.transform.GetComponent<PathingTile>().tileNode);
          }
        }
      }
    }

    var moveDirection = new Vector3();

    if (Input.GetKey(KeyCode.A))
    {
      moveDirection -= Vector3.right;
    }

    if (Input.GetKey(KeyCode.D))
    {
      moveDirection += Vector3.right;
    }

    if (Input.GetKey(KeyCode.W))
      moveDirection += Vector3.forward;

    if (Input.GetKey(KeyCode.S))
      moveDirection -= Vector3.forward;

    transform.position += moveDirection.normalized * 5 * Time.deltaTime;
  }
}