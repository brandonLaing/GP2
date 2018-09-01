using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
  public float moveSpeed = 1;

  public List<Vector3> moveToQue = new List<Vector3>();

  public Node startNode;

  public Node endNode;

  private void Update()
  {

    #region Camera Raycast
    if (Input.GetMouseButtonDown(1))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
        if (hit.transform.tag == "Tile")
        {
            hit.transform.gameObject.GetComponent<TileInfo>().SendCordinatesToAI();


        }
      }
    }

    if (Input.GetMouseButtonDown(0))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
        if (hit.transform.tag == "Tile")
        {
          hit.transform.gameObject.GetComponent<TileInfo>().MoveAIToCordinates();
          startNode = hit.transform.gameObject.GetComponent<TileInfo>().node;
          Debug.Log(startNode.transform.name);


        }
      }
    }
    #endregion

    #region Move to target
    if (moveToQue.Count > 0)
    {
      transform.position = Vector3.MoveTowards(transform.position, moveToQue[0], moveSpeed * Time.deltaTime);

      if (Vector3.Distance(transform.position, moveToQue[0]) < .1F)
      {
        moveToQue.Remove(moveToQue[0]);

      }
    }

    #endregion

    if (Input.GetKeyDown(KeyCode.Space))
    {
      List<Node> allNodeList = new List<Node>();

      foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
      {
        allNodeList.Add(tile.GetComponent<TileInfo>().node);

      }

      ShortestPathFinder.Dijkstra(startNode, endNode);

    }
  }

  public void AddPoint(Vector3 targetVector)
  {
    moveToQue.Add(targetVector);

  }
}

// make the object
// tile manger of the oject and set the new node to node you just created