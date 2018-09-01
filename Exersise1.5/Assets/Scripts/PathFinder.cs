using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
  public static List<Vector3> Dijkstra(Node startNode, Node endNode)
  {
    List<Vector3> waypoints = new List<Vector3>();

    // open list, closed list
    List<PathfindingNode> openList = new List<PathfindingNode>(), closedList = new List<PathfindingNode>();

    Dictionary<Node, PathfindingNode> pathfindingNodes = new Dictionary<Node, PathfindingNode>();

    pathfindingNodes.Add(startNode, new PathfindingNode(startNode));

    openList.Add(pathfindingNodes[startNode]);


    while (!DoesListContaiNode(endNode, closedList) && openList.Count > 0)
    {
      openList.Sort();
      PathfindingNode smallestCostSoFar = openList[0];

      foreach (Node connectedNode in smallestCostSoFar.graphNode.connections.Keys)
      {
        if (!DoesListContaiNode(connectedNode, closedList))
        {
          if (!DoesListContaiNode(connectedNode, openList))
          {
            float costToConnected = smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[connectedNode] + Vector3.Distance(connectedNode.transform.position, endNode.transform.position);
            PathfindingNode predecessor = smallestCostSoFar;

            pathfindingNodes.Add(connectedNode, new PathfindingNode(connectedNode, costToConnected, predecessor));
            openList.Add(pathfindingNodes[connectedNode]);


          }
          else
          {
            float currentCostToTarget = pathfindingNodes[connectedNode].costSoFar;
            float costToTargetThroughCurrentNode = smallestCostSoFar.costSoFar + smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[connectedNode];

            if (costToTargetThroughCurrentNode < currentCostToTarget)
            {
              pathfindingNodes[connectedNode].costSoFar = costToTargetThroughCurrentNode;
              pathfindingNodes[connectedNode].predecessor = smallestCostSoFar;
            }
          }
        }

      }
      closedList.Add(smallestCostSoFar);
      openList.Remove(smallestCostSoFar);

    }

    // destitionion is on closed list
    // all predecessors ar also on the closed list

    for (PathfindingNode waypoint = pathfindingNodes[endNode]; waypoint != null; waypoint = waypoint.predecessor)
    {
      waypoints.Add(waypoint.graphNode.transform.position);

    }

    waypoints.Reverse();


    return waypoints;
  }

  private static bool DoesListContaiNode(Node searchedNode, List<PathfindingNode> pathfindingNodeList)
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

}
