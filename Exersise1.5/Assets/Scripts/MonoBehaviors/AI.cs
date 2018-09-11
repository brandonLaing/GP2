using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** AI Does:
 * Moves to points across the map
 * These are given in multiple ways
 * either by player input or debug tools
 */
public class AI : MonoBehaviour
{
  public Node startNode;

  public int moveSpeed = 10;

  public List<Node> moveQue = new List<Node>();

  public List<Node> moveQueNodes = new List<Node>();

  public List<Node> allNodes = new List<Node>();

  private Node currentNode;

  public int randomPointsDebug = 1;

  public bool oneAtATime = false;

  public bool debug = false;

  public bool alwaysMove = true;

  private GameObject grabbedTile = null;

  public TileGenerator tileGen;

  private void Update()
  {
    // if there are more than one point in move Que move towards Que location
    // then if its close enough to the point remove it
    #region Move to Target
    if (moveQue.Count > 0)
    {
      Vector3 tempMove = moveQue[0].transform.position;
      tempMove.y = this.transform.position.y;

      transform.position = Vector3.MoveTowards(transform.position, tempMove, moveSpeed * Time.deltaTime);

      if (Vector3.Distance(transform.position, tempMove) < .1F)
      {
        if (moveQueNodes[0] == moveQue[0])
        {
          moveQueNodes.Remove(moveQueNodes[0]);

        }
        currentNode = moveQue[0];

        moveQue.Remove(moveQue[0]);

      }
    }
    #endregion

    // camera raycast to tiles
    #region Camera Raycast
    // if right click sends message to tile your hovering over to get a list of way points
    if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftShift))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
        if (hit.transform.tag == "Tile")
        {
          if (moveQue.Count == 0 || !oneAtATime)
          {
            hit.transform.gameObject.GetComponent<TileInfo>().SendCordinatesToAI(debug, this);
            moveQueNodes.Add(hit.transform.gameObject.GetComponent<TileInfo>().tileNode);

          }
        }
      }
    }

    // if left quick move AI to tile position
    if (Input.GetMouseButtonDown(0))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
        if (hit.transform.tag == "Tile")
        {
          AddNodeToList(hit.transform.GetComponent<TileInfo>());
          //hit.transform.gameObject.GetComponent<TileInfo>().MoveAIToCordinates(debug, this.transform);
          //startNode = hit.transform.gameObject.GetComponent<TileInfo>().tileNode;

        }
      }
    }

    // move cube
    if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
        if (hit.transform.tag == "Cube")
        {
          grabbedTile = hit.transform.gameObject;

        }
      }
    }

    if (Input.GetMouseButton(1) && grabbedTile != null)
    {
      tileGen.ReCheckConnections();

      Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

      temp.y = 0;

      grabbedTile.transform.position = temp;

    }

    if (Input.GetMouseButtonUp(1) && grabbedTile != null)
    {
      grabbedTile = null;

    }

    #endregion

    // debugging tools
    #region Debugging tests
    // gets a number of random points on the board and gets their path and starts to path find to them
    if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift))
    {
      for (int i = 0; i < randomPointsDebug; i++)
      {
        int randomNumber = Random.Range(0, allNodes.Count);

        AddNodeToList(allNodes[randomNumber].transform.gameObject.GetComponent<TileInfo>());
        //allNodes[randomNumber].transform.gameObject.GetComponent<TileInfo>().SendCordinatesToAI(debug, this);

      }
    }
    
    // tries to path find to every node on board
    if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift))
    {
      foreach(Node debugNode in allNodes)
      {
        debugNode.transform.gameObject.GetComponent<TileInfo>().SendCordinatesToAI(debug, this);

      }
    }

    // if the AI is set to always move when there are no way points left it generates a new way point
    if (moveQue.Count == 0 && alwaysMove)
    {
      int randomNumber = Random.Range(0, allNodes.Count);

      allNodes[randomNumber].transform.gameObject.GetComponent<TileInfo>().SendCordinatesToAI(debug, this);

    }
    #endregion

  }

  // if there AI spawns in a cube it resets its spawn
  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Cube")
    {
      if (moveQue.Count == 0)
      {
        tileGen.PickRandomStartLocationForAI();

      }

      else
      {
        this.transform.position = moveQue[0].transform.position;

      }
    }
  }

  private void RePathfind()
  {
    moveQue = new List<Node>();

    Node lastNode = currentNode;

    foreach (Node moveNode in moveQueNodes)
    {
      List<Node> nodeList = PathFinder.DijkstraNodes(lastNode, moveNode);

      foreach (Node waypointNode in nodeList)
      {
        moveQue.Add(waypointNode);
        lastNode = waypointNode;

      }
    }
  }

  private void AddNodeToList(TileInfo currentTileInfo)
  {
    currentTileInfo.SendCordinatesToAI(debug, this);

    List<Node> nodeList = PathFinder.DijkstraNodes(startNode, currentTileInfo.tileNode);

    foreach (Node node in nodeList)
    {
      moveQue.Add(node);

    }

    moveQueNodes.Add(currentTileInfo.tileNode);

    startNode = currentTileInfo.tileNode;

    if (debug)
    {
      currentTileInfo.tileNode.DisplayConnections();

    }
  }
}
