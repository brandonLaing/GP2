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
  public List<GameObject> selectedUnits;

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

    if (Input.GetMouseButtonDown(1) && currentMode == ControlMode.Build)
    {
      RaycastHit hit;

      if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, buildingLayer))
      {
        selectedTile = hit.transform.GetComponent<PathingTile>();

        selectedTile.BuildUnitSpawner();
      }
    }

    if (Input.GetMouseButton(0) && currentMode == ControlMode.Unit)
    {

    }
  }
}
