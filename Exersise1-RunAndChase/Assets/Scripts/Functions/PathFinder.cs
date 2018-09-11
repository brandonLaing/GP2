using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
  /**
  //public static List<Vector3> Dijkstra(Node startNode, Node endNode)
  //{   float starTime = Time.realtimeSinceStartup;
    
  //  List<Vector3> waypoints = new List<Vector3>();

  //  // open list, closed list
  //  List<PathfindingNode> openList = new List<PathfindingNode>(), closedList = new List<PathfindingNode>();

  //  Dictionary<Node, PathfindingNode> pathfindingNodes = new Dictionary<Node, PathfindingNode>();

  //  pathfindingNodes.Add(startNode, new PathfindingNode(startNode));

  //  openList.Add(pathfindingNodes[startNode]);


  //  while (!DoesListContainNode(endNode, closedList) && openList.Count > 0)
  //  {
  //    openList.Sort();
  //    PathfindingNode smallestCostSoFar = openList[0];

  //    foreach (Node connectedNode in smallestCostSoFar.graphNode.connections.Keys)
  //    {
  //      if (smallestCostSoFar.graphNode.connections[connectedNode] != 0)
  //      {
  //        if (!DoesListContainNode(connectedNode, closedList))
  //        {
  //          if (!DoesListContainNode(connectedNode, openList))
  //          {
  //            float costToConnected = smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[connectedNode] + Vector3.Distance(connectedNode.transform.position, endNode.transform.position);
  //            PathfindingNode predecessor = smallestCostSoFar;

  //            pathfindingNodes.Add(connectedNode, new PathfindingNode(connectedNode, costToConnected, predecessor));
  //            openList.Add(pathfindingNodes[connectedNode]);


  //          }
  //          else
  //          {
  //            float currentCostToTarget = pathfindingNodes[connectedNode].costSoFar;
  //            float costToTargetThroughCurrentNode = smallestCostSoFar.costSoFar +  smallestCostSoFar.graphNode.connections[connectedNode];

  //            if (costToTargetThroughCurrentNode < currentCostToTarget)
  //            {
  //              pathfindingNodes[connectedNode].costSoFar = costToTargetThroughCurrentNode;
  //              pathfindingNodes[connectedNode].predecessor = smallestCostSoFar;

  //            }
  //          }
  //        }
  //      }
  //    }

  //    closedList.Add(smallestCostSoFar);
  //    openList.Remove(smallestCostSoFar);

  //  }

  //  for (PathfindingNode waypoint = pathfindingNodes[endNode]; waypoint != null; waypoint = waypoint.predecessor)
  //  {
  //    waypoints.Add(waypoint.graphNode.transform.position);

  //  }

  //  waypoints.Reverse();

  //  //Debug.Log("Finding path took: " + (Time.realtimeSinceStartup - starTime) + "\n");

  //  return waypoints;
  //}
  */

  /**
  public static List<Node> DijkstraNodes(Node startNode, Node endNode)
  {
    float starTime = Time.realtimeSinceStartup;

    List<Node> waypoints = new List<Node>();

    // open list, closed list
    List<PathfindingNode> openList = new List<PathfindingNode>(), closedList = new List<PathfindingNode>();

    Dictionary<Node, PathfindingNode> pathfindingNodes = new Dictionary<Node, PathfindingNode>();

    pathfindingNodes.Add(startNode, new PathfindingNode(startNode));

    openList.Add(pathfindingNodes[startNode]);


    while (!DoesListContainNode(endNode, closedList) && openList.Count > 0)
    {
      openList.Sort();
      PathfindingNode smallestCostSoFar = openList[0];

      foreach (Node connectedNode in smallestCostSoFar.graphNode.connections.Keys)
      {
        if (smallestCostSoFar.graphNode.connections[connectedNode] != 0)
        {
          if (!DoesListContainNode(connectedNode, closedList))
          {
            if (!DoesListContainNode(connectedNode, openList))
            {
              float costToConnected = smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[connectedNode] + Vector3.Distance(connectedNode.transform.position, endNode.transform.position);
              PathfindingNode predecessor = smallestCostSoFar;

              pathfindingNodes.Add(connectedNode, new PathfindingNode(connectedNode, costToConnected, predecessor));
              openList.Add(pathfindingNodes[connectedNode]);


            }
            else
            {
              float currentCostToTarget = pathfindingNodes[connectedNode].costSoFar;
              float costToTargetThroughCurrentNode = smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[connectedNode];

              if (costToTargetThroughCurrentNode < currentCostToTarget)
              {
                pathfindingNodes[connectedNode].costSoFar = costToTargetThroughCurrentNode;
                pathfindingNodes[connectedNode].predecessor = smallestCostSoFar;

              }
            }
          }
        }
      }

      closedList.Add(smallestCostSoFar);
      openList.Remove(smallestCostSoFar);

    }

    for (PathfindingNode waypoint = pathfindingNodes[endNode]; waypoint != null; waypoint = waypoint.predecessor)
    {
      waypoints.Add(waypoint.graphNode);

    }

    waypoints.Reverse();

    //Debug.Log("Finding path took: " + (Time.realtimeSinceStartup - starTime) + "\n");

    return waypoints;
  }

  */

  public static List<Node> DijkstraNodes(Node startNode, Node endNode)
  {
    float starTime = Time.realtimeSinceStartup;

    List<Node> waypoints = new List<Node>();

    // open list, closed list
    List<PathfindingNode> openList = new List<PathfindingNode>(), closedList = new List<PathfindingNode>();

    Dictionary<Node, PathfindingNode> pathfindingNodes = new Dictionary<Node, PathfindingNode>();

    pathfindingNodes.Add(startNode, new PathfindingNode(startNode));

    openList.Add(pathfindingNodes[startNode]);


    while (!DoesListContainNode(endNode, closedList) && openList.Count > 0)
    {
      openList.Sort();
      PathfindingNode smallestCostSoFar = openList[0];

      for (int i = 0; i < smallestCostSoFar.graphNode.connections.Count; i++)
      {
        NodeConnection connectedNode = smallestCostSoFar.graphNode.connections[i];
        if (smallestCostSoFar.graphNode.connections[i].connected && !DoesListContainNode(connectedNode.node, closedList))
        {
          if (!DoesListContainNode(connectedNode.node, openList))
          {
            float costToConnect = smallestCostSoFar.costSoFar + connectedNode.cost + Vector3.Distance(connectedNode.node.nodeTransform.position, endNode.nodeTransform.position);
            PathfindingNode predecessor = smallestCostSoFar;

            pathfindingNodes.Add(connectedNode.node, new PathfindingNode(connectedNode.node, costToConnect, predecessor));
            openList.Add(pathfindingNodes[connectedNode.node]);

          } 
          else
          {
            float currenCostToTarget = pathfindingNodes[connectedNode.node].costSoFar;
            float costToTargetThoughCurretNode = smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[i].cost;

            if (costToTargetThoughCurretNode < currenCostToTarget)
            {
              pathfindingNodes[connectedNode.node].costSoFar = costToTargetThoughCurretNode;
              pathfindingNodes[connectedNode.node].predecessor = smallestCostSoFar;
            }

          }
        }
      }

      /**
      //foreach (Node connectedNode in smallestCostSoFar.graphNode.connections.Keys)
      //{
      //  if (smallestCostSoFar.graphNode.connections[connectedNode] != 0)
      //  {
      //    if (!DoesListContainNode(connectedNode, closedList))
      //    {
      //      if (!DoesListContainNode(connectedNode, openList))
      //      {
      //        float costToConnected = smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[connectedNode] + Vector3.Distance(connectedNode.transform.position, endNode.transform.position);
      //        PathfindingNode predecessor = smallestCostSoFar;

      //        pathfindingNodes.Add(connectedNode, new PathfindingNode(connectedNode, costToConnected, predecessor));
      //        openList.Add(pathfindingNodes[connectedNode]);


      //      }
      //      else
      //      {
      //        float currentCostToTarget = pathfindingNodes[connectedNode].costSoFar;
      //        float costToTargetThroughCurrentNode = smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[connectedNode];

      //        if (costToTargetThroughCurrentNode < currentCostToTarget)
      //        {
      //          pathfindingNodes[connectedNode].costSoFar = costToTargetThroughCurrentNode;
      //          pathfindingNodes[connectedNode].predecessor = smallestCostSoFar;

      //        }
      //      }
      //    }
      //  }
      //}
      */

      closedList.Add(smallestCostSoFar);
      openList.Remove(smallestCostSoFar);

    }

    for (PathfindingNode waypoint = pathfindingNodes[endNode]; waypoint != null; waypoint = waypoint.predecessor)
    {
      waypoints.Add(waypoint.graphNode);

    }

    waypoints.Reverse();

    //Debug.Log("Finding path took: " + (Time.realtimeSinceStartup - starTime) + "\n");

    return waypoints;
  }

  public static Node FindNextFurthestNode (Node currentNode,GameObject runnerAI, GameObject chaserAI)
  {
    List<Node> openList = new List<Node>(), closedList = new List<Node>();

    openList.Add(currentNode);

    Node destinatinNode = null;

    while (openList.Count > 0 && destinatinNode == null)
    {
      Node checkingNode = openList[0];
      Node furtherstNode = openList[0];

      for (int i = 0; i < checkingNode.connections.Count; i++)
      {
        NodeConnection connection = checkingNode.connections[i];

        if (checkingNode.connections[i].connected)
        {
          if (!DoesListContainNode(connection.node, closedList))
          {
            if (!DoesListContainNode(connection.node, openList))
            {
              if (Vector3.Distance(chaserAI.transform.position, currentNode.nodeTransform.position + Vector3.up) > Vector3.Distance(runnerAI.transform.position, chaserAI.transform.position))
              {
                furtherstNode = checkingNode;

              }
            }
          }
        }
      }

      if (furtherstNode != currentNode)
      {
        return furtherstNode;

      }

      /**
      foreach(Node connectionNode in checkingNode.connections.Keys)
      {
        if (checkingNode.connections[connectionNode] != 0)
        {
          if (!DoesListContainNode(connectionNode, closedList))
          {
            if (!DoesListContainNode(connectionNode, openList))
            {
              if (Vector3.Distance(chaserAI.transform.position, checkingNode.transform.position + Vector3.up) > Vector3.Distance(runnerAI.transform.position, chaserAI.transform.position))
              {
                return checkingNode;

              }

              openList.Add(connectionNode);

            }
          }
        }
      }
      */

      closedList.Add(checkingNode);
      openList.Remove(checkingNode);

    }

    return null;

  }

  private static bool DoesListContainNode(Node searchedNode, List<PathfindingNode> pathfindingNodeList)
  {
    foreach (PathfindingNode pathfindingNode in pathfindingNodeList)
    {
      if (pathfindingNode.graphNode == searchedNode)
      {
        return true;

      }
    }

    return false;

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
