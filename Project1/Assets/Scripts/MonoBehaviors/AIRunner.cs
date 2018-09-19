using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRunner : AIPathFinder
{
  [Header("Runner Specific")]
  public AIChaser chaser;

  private void Update()
  {
    moveQueDebug = moveQue.Count;
    moveWaypointsDebug = moveWaypoints.Count;

    ProgressOnMoveQue();

    if (moveQue.Count == 0 && Vector3.Distance(this.transform.position, chaser.transform.position) < 1000)
    {
      FindPathTo(FindNextFurthestNode(currentNode, this.gameObject, chaser.gameObject));
    }

  }

  public override void ProgressOnMoveQue()
  {
    if (moveQue.Count > 0)
    {
      Vector3 tempMove = moveQue[0].nodeTransform.position;
      tempMove.y = this.transform.position.y;

      transform.position = Vector3.MoveTowards(this.transform.position, tempMove, moveSpeed * Time.deltaTime);

      if (Vector3.Distance(transform.position, tempMove) < .1F)
      {
        currentNode = moveQue[0];
        moveQue.Remove(moveQue[0]);

      }
    }
  }

  public void FindNextFurthest()
  {
    Node furthest = currentNode;
    float furthestDistance = Vector3.Distance(this.transform.position, chaser.transform.position);

    foreach (NodeConnection connection in currentNode.connections)
    {
      if (Vector3.Distance(connection.node.nodeTransform.position + Vector3.up, chaser.transform.position) > furthestDistance && connection.connected)
      {
        furthest = connection.node;
        furthestDistance = Vector3.Distance(connection.node.nodeTransform.position + Vector3.up, chaser.transform.position);

      }
    }

    if (furthest != currentNode)
    {
      moveQue.Add(furthest);

    }
  }

  public static Node FindNextFurthestNode(Node currentNode, GameObject runnerAI, GameObject chaserAI)
  {
    List<Node> openList = new List<Node>(), closedList = new List<Node>();

    openList.Add(currentNode);

    Node destinatinNode = null;

    while (openList.Count > 0 && destinatinNode == null)
    {
      Node checkingNode = openList[0];

      foreach (NodeConnection connectionNode in checkingNode.connections)
      {
        if (connectionNode.connected)
        {
          if (!DoesListContainNode(connectionNode.node, closedList))
          {
            if (!DoesListContainNode(connectionNode.node, openList))
            {
              if (Vector3.Distance(chaserAI.transform.position, checkingNode.nodeTransform.position + Vector3.up) > Vector3.Distance(runnerAI.transform.position, chaserAI.transform.position))
              {
                return checkingNode;

              }

              openList.Add(connectionNode.node);

            }
          }
        }
      }

      closedList.Add(checkingNode);
      openList.Remove(checkingNode);

    }

    return null;
  }

  private static bool DoesListContainNode(Node searchedNode, List<Node> nodeList)
  {
    foreach (Node pathfindingNode in nodeList)
    {
      if (pathfindingNode == searchedNode)
      {
        return true;
      }
    }

    return false;
  }


}
