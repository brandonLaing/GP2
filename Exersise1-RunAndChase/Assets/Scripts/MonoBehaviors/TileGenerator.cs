using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TileGenerator : MonoBehaviour
{
  public int gridWidth;
  public int gridHeight;

  public AIPathFinder[] pathfinder;
  public AIChaser chaser;
  public AIRunner runner;

  public GameObject tilePrefab;

  public Material matBlack;
  public Material matWhite;

  public CubeSetter cubeSetter;

  public int[] deadZoneX;
  public int[] deadZoneZ;

  private List<GizmoConnect> hitConnections = new List<GizmoConnect>();
  private List<GizmoConnect> missConnections = new List<GizmoConnect>();

  public static Node[] allNodes = new Node[0];

  private void Start()
  {
    float startTime = Time.realtimeSinceStartup;

    MakeAllTiles();

    cubeSetter.SetCubes(gridWidth, gridHeight);

    SetAIOntoBoard();

    Debug.Log("Start Generation took: " + (Time.realtimeSinceStartup - startTime) + "\n");

    foreach(AIPathFinder path in pathfinder)
    {
      path.RebuildPath();

    }

    chaser.RebuildPath();
    runner.RebuildPath();

    RecheckConnections();

  }

  private void Update()
  {
    
  }

  // Makes all the tiles
  private void MakeAllTiles()
  {
    List<Node> allNodesList = new List<Node>();

    int i = 0;

    for (int x = 0; x < gridWidth; x++)
    {
      for (int z = 0; z < gridHeight; z++)
      {
        if (!(deadZoneX.Contains(x) && deadZoneZ.Contains(z)))
        {
          #region Make Tile and Set Up Node
          GameObject newTile = GameObject.Instantiate(tilePrefab, new Vector3(x, 0, z), tilePrefab.transform.rotation, this.transform);
          newTile.transform.name += " " + i;
          newTile.GetComponent<TileInfo>().tileNode = new Node(newTile.transform);


          allNodesList.Insert(0, newTile.GetComponent<TileInfo>().tileNode);
          #endregion

          #region CheckConnections
          int connectinsSoFar = 0;

          Node baseNode = newTile.GetComponent<TileInfo>().tileNode;

          foreach (Node crossNode in allNodesList)
          {
            if (connectinsSoFar < 2)
            {
              if (baseNode.nodeTransform.position == crossNode.nodeTransform.position + Vector3.forward ||
                  baseNode.nodeTransform.position == crossNode.nodeTransform.position + Vector3.right)
              {
                ++connectinsSoFar;

                Vector3 tempBaseNode = baseNode.nodeTransform.position + Vector3.up;
                Vector3 tempCrossNode = crossNode.nodeTransform.position + Vector3.up;

                Ray ray = new Ray(tempBaseNode, -(tempBaseNode - tempCrossNode));
                Ray rayReversed = new Ray(tempCrossNode, -(tempCrossNode - tempBaseNode));

                if (Physics.Raycast(ray, 1) || Physics.Raycast(rayReversed, 1))
                {
                  hitConnections.Add(new GizmoConnect(baseNode, crossNode));

                  baseNode.connections.Add(new NodeConnection(crossNode, 0, false));
                  crossNode.connections.Add(new NodeConnection(crossNode, 0, false));

                }
                else
                {
                  missConnections.Add(new GizmoConnect(baseNode, crossNode));

                  baseNode.connections.Add(new NodeConnection(crossNode, 1, true));
                  crossNode.connections.Add(new NodeConnection(baseNode, 1, true));

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
              newTile.GetComponent<MeshRenderer>().material = matBlack;

            }
            else
            {
              newTile.GetComponent<MeshRenderer>().material = matWhite;

            }
          }

          else
          {
            if (z % 2 == 0)
            {
              newTile.GetComponent<MeshRenderer>().material = matWhite;

            }
            else
            {
              newTile.GetComponent<MeshRenderer>().material = matBlack;

            }

          }
          #endregion

        }
      }
    }

    allNodes = allNodesList.ToArray();

  }

  // Sets all ai onto the board and gives a current location
  private void SetAIOntoBoard()
  {
    for (int i = 0; i < pathfinder.Length; i++)
    {
      if (pathfinder[i] != null)
      {
        SetStartingNode(pathfinder[i]);

      }
    }

    if (chaser != null)
    {
      SetStartingNode(chaser);

    }

    if (runner != null)
    {
      SetStartingNode(runner);

    }
  }

  #region Set AI on start positions
  private void SetStartingNode(AIPathFinder ai)
  {
    ai.currentNode = GetRandomStartLocation(ai.gameObject);

  }

  #endregion

  // moves object to tile then returns the node for that tile
  public static Node GetRandomStartLocation(GameObject objectToSet)
  {
    int randomNumber = UnityEngine.Random.Range(0, allNodes.Length);

    allNodes[randomNumber].nodeTransform.GetComponent<TileInfo>().MoveObjectsToPosition(objectToSet.transform);

    return allNodes[randomNumber].nodeTransform.GetComponent<TileInfo>().tileNode;
  }

  // rechecks connections to make sure they are still valid
  public void RecheckConnections()
  {
    hitConnections = new List<GizmoConnect>();
    missConnections = new List<GizmoConnect>();

    for (int i = 0; i < allNodes.Length; i++)
    {
      Node baseNode = allNodes[i];

      var buffer = new List<NodeConnection>(baseNode.connections);

      for (int j = 0; j < buffer.Count; j++)
      {
        Node connection = baseNode.connections[j].node;

        int basePositionInConnection = FindPositionInNodeConnection(baseNode, connection);

        if (baseNode.nodeTransform.position == connection.nodeTransform.position + Vector3.forward ||
            baseNode.nodeTransform.position == connection.nodeTransform.position + Vector3.right)
        {
          Ray ray = new Ray(baseNode.nodeTransform.position, -(baseNode.nodeTransform.position - connection.nodeTransform.position));
          Ray rayReversed = new Ray(connection.nodeTransform.position, -(connection.nodeTransform.position - baseNode.nodeTransform.position));

          if (Physics.Raycast(ray, 1) || Physics.Raycast(rayReversed, 1))
          {
            baseNode.connections[j].connected = false;
            baseNode.connections[j].cost = 0;

            connection.connections[basePositionInConnection].connected = false;
            connection.connections[basePositionInConnection].cost = 0;

            hitConnections.Add(new GizmoConnect(baseNode, baseNode.connections[j].node));

          }
          else
          {
            baseNode.connections[j].connected = true;
            baseNode.connections[j].cost = 1;

            connection.connections[basePositionInConnection].connected = true;
            connection.connections[basePositionInConnection].cost = 1;

            missConnections.Add(new GizmoConnect(baseNode, connection));

          }
        }
      }
    }
  }

  public int FindPositionInNodeConnection(Node baseNode, Node connection)
  {
    for (int k = 0; k < connection.connections.Capacity; k++)
    {
      if (baseNode == connection.connections[k].node)
      {
        return k;
      }
    }

    Debug.Log("Why Does this work");

    return 0;
  }

  // draws lines and CIRCLES!!!!!!!!!!!!!
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.black;
    try
    {
      if (allNodes.Length > 0)
      {
        for (int i = 0; i < allNodes.Length; i++)
        {
          Gizmos.DrawSphere(allNodes[i].nodeTransform.position + Vector3.up, .2F);

        }

        Gizmos.color = Color.green;
        foreach (GizmoConnect connecetion in missConnections)
        {
          Gizmos.DrawLine(connecetion.positionOne, connecetion.positionTwo);

        }

        Gizmos.color = Color.red;
        foreach (GizmoConnect connection in hitConnections)
        {
          Gizmos.DrawLine(connection.positionOne, connection.positionTwo);

        }
      }
    } catch
    {
      if (MouseClicks.doDebug)
      {
        Debug.Log("Can't Draw your Gizmos Right Now\n");

      }
    }
  }
}
