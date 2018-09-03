using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
  public int gridWidth = 10;
  public int gridHeight = 10;

  public AI aiPlayer;
  public RandonCubeSetter cubeSetter;

  public GameObject tileTemplate;

  public List<Node> allNodes = new List<Node>();

  public Material black;
  public Material white;

  private List<GizmoConnect> hitConnections = new List<GizmoConnect>();
  private List<GizmoConnect> missConnections = new List<GizmoConnect>();

  public List<int> xDeadZone = new List<int>();
  public List<int> zDeadZone = new List<int>();

  private List<Ray> rays = new List<Ray>();

  void Start()
  {
    float starTime = Time.realtimeSinceStartup;

    MapReset.tileGen = this;

    aiPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<AI>();

    // Makes all cubes on map
    cubeSetter.MakeCubes();

    int i = 0;    // i is the number of the cube thats being created. This is for naming
    for (int x = 0; x < gridWidth; x++)
    {
      for (int z = 0; z < gridHeight; z++)
      {
        // if the current position isnt in a dead zone
        if (!(xDeadZone.Contains(x) && zDeadZone.Contains(z)))
        {
          // this makes a new tile, sets it name, increments the current tile number, creates new node for tile, and inserts the new tile node in the front of the line
          // putting it into the front of the list is so when the system goes though and tries to find the other two connections it has made so far it goes though fewer items
          // to find its connection
          #region Make Tiles
          GameObject newTile = GameObject.Instantiate(tileTemplate, new Vector3(x, 0, z), tileTemplate.transform.rotation, this.gameObject.transform);

          newTile.transform.name += " " + i;

          i++;

          newTile.GetComponent<TileInfo>().tileNode = new Node(newTile.transform);

          allNodes.Insert(0, newTile.GetComponent<TileInfo>().tileNode);    // average time to build .04 seconds
          //allNodes.Add(newTile.GetComponent<TileInfo>().tileNode);        // average time to build .1  seconds

          #endregion

          // this goes though all nodes made so far and compares their position to the node that was just created
          #region Check Connections
          int temp = 0;     // number of connections found so far

          Node nodeChecker = newTile.GetComponent<TileInfo>().tileNode;

          foreach (Node nodeChecked in allNodes)
          {
            // if you have gotten less than two connections
            if (temp < 2)
            {
              // if the secondary node your checking against is one left or bellow continue though
              if (nodeChecker.transform.position == nodeChecked.transform.position + new Vector3(0, 0, 1) ||
                  nodeChecker.transform.position == nodeChecked.transform.position + new Vector3(1, 0, 0))
              {
                // add one to number of nodes checked
                temp++;

                // make a ray from base node to your secondary node? I'm not sure on this math but it works ¯\_(ツ)_/¯
                Ray ray = new Ray(nodeChecker.transform.position, -(nodeChecker.transform.position - nodeChecked.transform.position));
                // then make another ray from the secondary node to the base node
                Ray rayReversed = new Ray(nodeChecked.transform.position, -(nodeChecked.transform.position - nodeChecker.transform.position));

                // check if either of the raycast hits
                // the reason we do this is cause the point may be inside a box and ray casting out of a box wouldn't cause a hit
                if (Physics.Raycast(ray, 1) || Physics.Raycast(rayReversed, 1))
                {
                  // for gizmo add this hit to hit connections
                  hitConnections.Add(new GizmoConnect(nodeChecker, nodeChecked));

                }
                else
                {
                  // for gizmo add this to miss connections
                  missConnections.Add(new GizmoConnect(nodeChecker, nodeChecked));

                  // this means there was no hit so add connections between the two nodes
                  nodeChecker.connections.Add(nodeChecked, 1);
                  nodeChecked.connections.Add(nodeChecker, 1);

                }
              }
            }
          }
          #endregion

          // sets the color for every tile in a checker mark style
          #region Set Color
          if (x % 2 == 0)
          {
            if (z % 2 == 0)
            {
              newTile.GetComponent<MeshRenderer>().material = black;

            }
            else
            {
              newTile.GetComponent<MeshRenderer>().material = white;

            }
          }

          else
          {
            if (z % 2 == 0)
            {
              newTile.GetComponent<MeshRenderer>().material = white;

            }
            else
            {
              newTile.GetComponent<MeshRenderer>().material = black;

            }
          }
          #endregion
        }
      }
    }

    #region Pick Random Start and set camera
    // set a random start location for the Ai
    PickRandomStartLocation();
    #endregion

    float endTime = Time.realtimeSinceStartup;
    Debug.Log("Start Generation took: " + (endTime - starTime));

  }

  public void PickRandomStartLocation()
  {
    // get a random number between 0 and the number of nodes
    int randomNumber = Random.Range(0, allNodes.Count);

    // send set player to nodes position
    allNodes[randomNumber].transform.GetComponent<TileInfo>().MoveAIToCordinates(aiPlayer.debug, aiPlayer);

    // set start node to this node
    aiPlayer.startNode = allNodes[randomNumber].transform.GetComponent<TileInfo>().tileNode;

    // set the AIs all nodes to all nodes
    aiPlayer.allNodes = allNodes;

    // set the cameras location
    Camera.main.transform.position = new Vector3(gridWidth / 2, Camera.main.transform.position.y, gridHeight / 2);

  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.black;
    if (allNodes.Count > 0)
    {
      foreach (Node allNode in allNodes)
      {

        Gizmos.DrawSphere(allNode.transform.position + new Vector3(0, 1, 0), .2F);

      }

      foreach (GizmoConnect connection in missConnections)
      {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(connection.node1, connection.node2);

      }

      foreach (GizmoConnect connection in hitConnections)
      {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(connection.node1, connection.node2);

      }

    }
  }

  public void ReCheckConnections()
  {
    hitConnections = new List<GizmoConnect>();
    missConnections = new List<GizmoConnect>();

    foreach (Node baseNode in allNodes)
    {
      baseNode.connections = new Dictionary<Node, float>();

    }

    foreach (Node baseNode in allNodes)
    {
      int temp = 0;
      foreach (Node secondaryNode in allNodes)
      {
        if (temp < 2)
        {
          if (baseNode.transform.position == secondaryNode.transform.position + Vector3.forward ||
              baseNode.transform.position == secondaryNode.transform.position + Vector3.right)
          {
            temp++;

            Ray toRay = new Ray(baseNode.transform.position, -(baseNode.transform.position - secondaryNode.transform.position));
            Ray backRay = new Ray(secondaryNode.transform.position, -(secondaryNode.transform.position - baseNode.transform.position));

            if (Physics.Raycast(toRay, 1) || Physics.Raycast(backRay, 1))
            {
              hitConnections.Add(new GizmoConnect(baseNode, secondaryNode));

            }
            else
            {
              missConnections.Add(new GizmoConnect(baseNode, secondaryNode));

              baseNode.connections.Add(secondaryNode, 1);
              secondaryNode.connections.Add(baseNode, 1);

            }
          }
        }
      }
    }
  }

}