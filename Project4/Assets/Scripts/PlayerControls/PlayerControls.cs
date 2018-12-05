using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
  public LayerMask unitLayer;

  public LayerMask buildingLayer;
  public PathingTile selectedTile;

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      RaycastHit hit;

      if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, buildingLayer))
      {
        selectedTile = hit.transform.GetComponent<PathingTile>();

        var sb = new System.Text.StringBuilder();

        foreach (var actionName in selectedTile.actions.Keys)
        {
          sb.Append(actionName);
        }

        Debug.Log(sb);
      }
    }

    if (selectedTile != null)
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        selectedTile.actions["Build Unit Spawner"]();
      }

    }
  }
}
