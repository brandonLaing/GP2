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

  public List<Vector3> moveQue = new List<Vector3>();

  public List<Node> allNodes = new List<Node>();

  public int randomPointsDebug = 1;

  public bool oneAtATime = false;

  public bool debug = false;

  public bool alwaysMove = true;

  public TileGenerator tileGen;

  private void Update()
  {
    // if there are more than one point in move Que move towards Que location
    // then if its close enough to the point remove it
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

    // camera raycast to tiles
    #region Camera Raycast
    // if right click sends message to tile your hovering over to get a list of way points
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
            hit.transform.gameObject.GetComponent<TileInfo>().SendCordinatesToAI(debug, this);

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
          hit.transform.gameObject.GetComponent<TileInfo>().MoveAIToCordinates(debug, this);
          startNode = hit.transform.gameObject.GetComponent<TileInfo>().tileNode;

        }
      }
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

        allNodes[randomNumber].transform.gameObject.GetComponent<TileInfo>().SendCordinatesToAI(debug, this);

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

    // if the AI is set to always move when there are no way points left it generates a new waypoint
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
      tileGen.PickRandomStartLocation();
    }
  }
}
