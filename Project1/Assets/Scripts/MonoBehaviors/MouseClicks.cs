using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClicks : MonoBehaviour
{
  public AIPathFinder aiAgent;

  public AIPathFinder[] aiAgentsForUpdating;

  public TileGenerator tileGen;

  public static bool doDebug = false;

  private GameObject grabbedCube;

  void Update()
  {
    #region Click to Move for aiAgent
    if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftShift))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
        if (hit.transform.tag == "Tile")
        {
          aiAgent.FindPathTo(hit.transform.GetComponent<TileInfo>());

        }
      }
    }
    #endregion

    #region Move aiAgent to location
    if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;
      
      if (Physics.Raycast(ray, out hit))
      {
        if (hit.transform.tag == "Tile")
        {
          aiAgent.MoveDirectlyToNode(hit.transform.GetComponent<TileInfo>().tileNode);

        }
      }
    }
    #endregion

    #region Move Blocks
    // if you click on the cube add it to your cube object
    if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;
      
      if (Physics.Raycast(ray, out hit))
      {
        if (hit.transform.tag == "Cube")
        {
          grabbedCube = hit.transform.gameObject;

        }
      }
    } 

    // while you have mouse right down
    if (Input.GetMouseButton(1) && grabbedCube != null)
    {
      tileGen.RecheckConnections();

      for (int i = 0; i < aiAgentsForUpdating.Length; i++)
      {
        aiAgentsForUpdating[i].RebuildPath();

      }

      Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

      temp.y = grabbedCube.transform.position.y;

      grabbedCube.transform.position = temp;
    }

    // if you let go of cube
    if (Input.GetMouseButtonUp(1) && grabbedCube != null)
    {
      Vector3 temp = new Vector3();
      temp.x = Mathf.RoundToInt(grabbedCube.transform.position.x);
      temp.y = grabbedCube.transform.position.y;
      temp.z = Mathf.RoundToInt(grabbedCube.transform.position.z);

      grabbedCube.transform.position = temp;

      grabbedCube = null;

    }
    #endregion

  }
}
