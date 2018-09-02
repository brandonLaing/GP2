using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
  public Node startNode;

  public int moveSpeed = 10;

  public List<Vector3> moveQue = new List<Vector3>();

  public List<Node> allNodes = new List<Node>();

  public int randomPointsDebug = 1;

  public bool oneAtATime = false;

  private void Start()
  {
    TileInfo.ai = this;
    TileGenerator.aiPlayer = this;

  }

  private void Update()
  {
    #region Move to Target
    if (moveQue.Count > 0)
    {
      Vector3 tempMove = moveQue[0];
      tempMove.y = this.transform.position.y;

      transform.position = Vector3.MoveTowards(transform.position, tempMove, moveSpeed * Time.deltaTime);

      if (Vector3.Distance(transform.position, tempMove) < .1F)
      {
        moveQue.Remove(moveQue[0]);

      }
    }
    #endregion

    #region Camera Raycast
    if (Input.GetMouseButtonDown(1))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
        if (hit.transform.tag == "Tile")
        {
          if (moveQue.Count == 0 || !oneAtATime)
          {
            hit.transform.gameObject.GetComponent<TileInfo>().SendCordinatesToAI();

          }
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
          startNode = hit.transform.gameObject.GetComponent<TileInfo>().tileNode;

        }
      }
    }
    #endregion

    if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift))
    {
      for (int i = 0; i < randomPointsDebug; i++)
      {
        int randomNumber = Random.Range(0, allNodes.Count);

        allNodes[randomNumber].transform.gameObject.GetComponent<TileInfo>().SendCordinatesToAI();

      }
    }

    if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift))
    {
      foreach(Node debugNode in allNodes)
      {
        debugNode.transform.gameObject.GetComponent<TileInfo>().SendCordinatesToAI();

      }
    }

  }
}
