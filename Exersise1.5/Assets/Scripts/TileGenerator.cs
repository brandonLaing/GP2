using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
  public int gridWidth = 10;
  public int gridHeight = 10;

  public AI aiPlayer;

  public GameObject tileTemplate;

  public List<Node> allNodes = new List<Node>();

  public Material black;
  public Material white;

  private List<GizmoConnect> hitConnections = new List<GizmoConnect>();
  private List<GizmoConnect> missConnections = new List<GizmoConnect>();

  private List<Ray> rays = new List<Ray>();

  void Start()
  {
    MapReset.tileGen = this;

    aiPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<AI>();

    int i = 0;
    for (int x = 0; x < gridWidth; x++)
    {
      for (int z = 0; z < gridHeight; z++)
      {
        #region Make Tiles
        GameObject newTile = GameObject.Instantiate(tileTemplate, new Vector3(x, 0, z), tileTemplate.transform.rotation, this.gameObject.transform);

        newTile.transform.name += " " + i;

        newTile.GetComponent<TileInfo>().tileNode = new Node(newTile.transform);

        allNodes.Insert(0, newTile.GetComponent<TileInfo>().tileNode);

        i++;
        #endregion

        // Needs Fixing
        #region Check Connections
        int temp = 0;

        Node nodeChecker = newTile.GetComponent<TileInfo>().tileNode;
        foreach (Node nodeChecked in allNodes)
        {
          if (temp < 2)
          {
            if (nodeChecker.transform.position == nodeChecked.transform.position + new Vector3(0, 0, 1) ||
                nodeChecker.transform.position == nodeChecked.transform.position + new Vector3(1, 0, 0))
            {
              temp++;

              Ray ray = new Ray(nodeChecker.transform.position, -(nodeChecker.transform.position - nodeChecked.transform.position));
              Ray rayReversed = new Ray(nodeChecked.transform.position, -(nodeChecked.transform.position - nodeChecker.transform.position));

              if (Physics.Raycast(ray, 1) || Physics.Raycast(rayReversed, 1))
              {
                hitConnections.Add(new GizmoConnect(nodeChecker, nodeChecked));

              }
              else
              {
                missConnections.Add(new GizmoConnect(nodeChecker, nodeChecked));

                nodeChecker.connections.Add(nodeChecked, 1);
                nodeChecked.connections.Add(nodeChecker, 1);

              }
            }
          }
        }
        #endregion

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

    #region Pick Random Start and set camera
    int randomNumber = Random.Range(0, allNodes.Count);

    allNodes[randomNumber].transform.GetComponent<TileInfo>().ai = aiPlayer;

    allNodes[randomNumber].transform.GetComponent<TileInfo>().MoveAIToCordinates();

    aiPlayer.startNode = allNodes[randomNumber].transform.GetComponent<TileInfo>().tileNode;

    aiPlayer.allNodes = allNodes;

    Camera.main.transform.position = new Vector3(gridWidth / 2, Camera.main.transform.position.y, gridHeight / 2);

    #endregion
  
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