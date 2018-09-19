using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathFinder : MonoBehaviour
{
  public int moveSpeed;

  public List<Node> moveQue = new List<Node>();
  public int moveQueDebug = 0;

  public List<Node> moveWaypoints = new List<Node>();
  public int moveWaypointsDebug = 0;

  public Node currentNode;

  [Header("Debug")]
  public bool alwaysMove;
	
	// Update is called once per frame
	void Update ()
  {
    moveQueDebug = moveQue.Count;
    moveWaypointsDebug = moveWaypoints.Count;

    ProgressOnMoveQue();
    AlwaysMove();

	}

  // moves to closest node on moveQue
  public virtual void ProgressOnMoveQue()
  {
    if (moveQue.Count > 0)
    {
      Vector3 tempMove = moveQue[0].nodeTransform.position;
      tempMove.y = this.transform.position.y;

      transform.position = Vector3.MoveTowards(this.transform.position, tempMove, moveSpeed * Time.deltaTime);

      if (Vector3.Distance(transform.position, tempMove) < .1F)
      {
        if (moveQue[0] == moveWaypoints[0] && (moveQue.Count > 0 && moveWaypoints.Count > 0))
        {
          moveWaypoints.Remove(moveWaypoints[0]);

        }

        currentNode = moveQue[0];
        moveQue.Remove(moveQue[0]);

      }
    }
  }

  public void AlwaysMove()
  {
    if (moveQue.Count == 0 && alwaysMove)
    {
      FindPathTo(GetRandomNode());

    }
  }

  public Node GetRandomNode()
  {
    int randomNumber = Random.Range(0, TileGenerator.allNodes.Length);
    return TileGenerator.allNodes[randomNumber];
  }

  public void FindPathTo(TileInfo tileInfo)
  {
    if (MouseClicks.doDebug)
    {
      tileInfo.tileNode.DisplayConnections();
    }

    try
    {
      List<Node> nodeList;

      if (moveWaypoints.Count == 0)
      {
        nodeList = PathFinder.DijkstraNodes(currentNode, tileInfo.tileNode);

      }
      else
      {
        nodeList = PathFinder.DijkstraNodes(moveWaypoints[moveWaypoints.Count - 1], tileInfo.tileNode);

      }

      foreach (Node node in nodeList)
      {
        moveQue.Add(node);

      }

      moveWaypoints.Add(tileInfo.tileNode);
    }
    catch
    {
      if (MouseClicks.doDebug)
      {
        Debug.LogWarning("Couldn't find path to " + tileInfo.name + "\n");

      }
    }
  }

  public void FindPathTo(Node node)
  {
    try
    {
      List<Node> nodeList;

      if (moveWaypoints.Count == 0)
      {
        nodeList = PathFinder.DijkstraNodes(currentNode, node);

      }
      else
      {
        nodeList = PathFinder.DijkstraNodes(moveWaypoints[moveWaypoints.Count - 1], node);

      }

      foreach (Node nodeInList in nodeList)
      {
        moveQue.Add(nodeInList);

      }

      moveWaypoints.Add(node);

    }
    catch
    {
      if (MouseClicks.doDebug)
      {
        Debug.LogWarning("Couldn't find path to " + node.nodeTransform.name + "\n");

      }
    }
  }

  public void MoveDirectlyToNode(Node node)
  {
    currentNode = node;
    node.nodeTransform.GetComponent<TileInfo>().MoveObjectsToPosition(this.transform);

  }

  // Needs Improvements
  public virtual void RebuildPath()
  {
    try
    {
      if (moveQue.Count > 0)
      {
        Node lastPosition = moveQue[0];
        moveQue = new List<Node>();

        foreach (Node waypoint in moveWaypoints)
        {
          List<Node> nodeList;

          nodeList = PathFinder.DijkstraNodes(lastPosition, waypoint);

          foreach (Node listNode in nodeList)
          {
            moveQue.Add(listNode);
            lastPosition = listNode;

          }
        }
      }
    }
    catch
    {
      Debug.LogWarning("Couldn't rebuild path " + "\n");

    }
  }

  public virtual void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Cube")
    {
      if (moveQue.Count == 0)
      {
        currentNode = TileGenerator.GetRandomStartLocation(this.gameObject);

      }
      else
      {
        transform.position = moveQue[0].nodeTransform.position + Vector3.up;

      }
    }
  }
}
