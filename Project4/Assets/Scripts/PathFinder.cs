using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinder
{
  public static List<PathFindingNode> DijkstraNodes(PathFindingNode startNode, PathFindingNode endNode)
  {
    float starTime = Time.realtimeSinceStartup;

    List<PathFindingNode> waypoints = new List<PathFindingNode>();

    // open list, closed list
    List<PathfinderNode> openList = new List<PathfinderNode>(), closedList = new List<PathfinderNode>();

    Dictionary<PathFindingNode, PathfinderNode> pathfindingNodes = new Dictionary<PathFindingNode, PathfinderNode>();

    pathfindingNodes.Add(startNode, new PathfinderNode(startNode));

    openList.Add(pathfindingNodes[startNode]);


    while (!DoesListContainNode(endNode, closedList) && openList.Count > 0)
    {
      openList.Sort();
      PathfinderNode smallestCostSoFar = openList[0];

      for (int i = 0; i < smallestCostSoFar.graphNode.connections.Length; i++)
      {
        if (smallestCostSoFar.graphNode.connections[i] != null)
        {
          PathfindingNodeConnection connectedNode = smallestCostSoFar.graphNode.connections[i];

          if (smallestCostSoFar.graphNode.connections[i].IsConnected && !DoesListContainNode(connectedNode.endNode, closedList))
          {
            if (!DoesListContainNode(connectedNode.endNode, openList))
            {
              float costToConnect = smallestCostSoFar.costSoFar + connectedNode.cost + Vector3.Distance(connectedNode.endNode.nodeTransform.position, endNode.nodeTransform.position);
              PathfinderNode predecessor = smallestCostSoFar;

              pathfindingNodes.Add(connectedNode.endNode, new PathfinderNode(connectedNode.endNode, costToConnect, predecessor));
              openList.Add(pathfindingNodes[connectedNode.endNode]);

            }
            else
            {
              float currenCostToTarget = pathfindingNodes[connectedNode.endNode].costSoFar;
              float costToTargetThoughCurretNode = smallestCostSoFar.costSoFar + smallestCostSoFar.graphNode.connections[i].cost;

              if (costToTargetThoughCurretNode < currenCostToTarget)
              {
                pathfindingNodes[connectedNode.endNode].costSoFar = costToTargetThoughCurretNode;
                pathfindingNodes[connectedNode.endNode].predecessor = smallestCostSoFar;
              }

            }
          }
        }
      }

      closedList.Add(smallestCostSoFar);
      openList.Remove(smallestCostSoFar);

    }

    for (PathfinderNode waypoint = pathfindingNodes[endNode]; waypoint != null; waypoint = waypoint.predecessor)
    {
      waypoints.Add(waypoint.graphNode);
    }

    waypoints.Reverse();

    return waypoints;
  }

  private static bool DoesListContainNode(PathFindingNode searchedNode, List<PathfinderNode> pathfindingNodeList)
  {
    foreach (PathfinderNode pathfindingNode in pathfindingNodeList)
    {
      if (pathfindingNode.graphNode == searchedNode)
      {
        return true;

      }
    }

    return false;

  }

  private static bool DoesListContainNode(PathFindingNode searchedNode, List<PathFindingNode> nodeList)
  {
    foreach (PathFindingNode pathfindingNode in nodeList)
    {
      if (pathfindingNode == searchedNode)
      {
        return true;
      }
    }

    return false;
  }

}

public class PathfinderNode : IComparable<PathfinderNode>
{
  public PathFindingNode graphNode;

  public float costSoFar;

  public PathfinderNode predecessor;


  public PathfinderNode(PathFindingNode newNode)
  {
    graphNode = newNode;
    costSoFar = 0;

  }

  public PathfinderNode(PathFindingNode graphNode, float costSoFar, PathfinderNode predecessor)
  {
    this.graphNode = graphNode;
    this.costSoFar = costSoFar;
    this.predecessor = predecessor;
  }

  public int CompareTo(PathfinderNode other)
  {
    return costSoFar.CompareTo(other.costSoFar);

  }

}
