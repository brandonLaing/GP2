using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{ 

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
            // SS: I haven't traced the behavior, but this is definitely incorrect.
            // costToConnect will ultimately become the costSoFar attached to a PathfindingNode,
            // and it adds the previous node's costSoFar, plus the connection, plus the heuristic estimate,
            // giving an estimated total path length. And then, when this node becomes the smallest on the
            // open list, costToConnect will be greatly overestimated, since smallestCostSoFar.costSoFar
            // will be nearly the full length of the path, and then you're adding on an additional
            // connection cost as well as an additional (if slightly reduced) heuristic estimate.
            //
            // I expect this will result in Dikstra-like behavior, because the further a node is along
            // the path, the more overestimated its overall cost is, making it less likely to sort as
            // the smallest cost so far, giving preference to nodes further back, resulting in 
            // increased fill (many more nodes being considered than need to be to return an optimal path).
            // This is why the frame rate drops below 60 FPS when many agents pathfind in the same frame.
            //
            // Your PathfindingNode should keep the true cost and the heuristic cost separate to prevent this.

            float costToConnect = smallestCostSoFar.costSoFar + connectedNode.cost + Vector3.Distance(connectedNode.node.nodeTransform.position, endNode.nodeTransform.position);
            PathfindingNode predecessor = smallestCostSoFar;

            pathfindingNodes.Add(connectedNode.node, new PathfindingNode(connectedNode.node, costToConnect, predecessor));
            openList.Add(pathfindingNodes[connectedNode.node]);

          } 
          else
          {
            // SS: As above, your costSoFar values represent the complete path, not the known cost so far,
            // and because of the previous errors, it's possible for this to cause sub-optimal paths to be
            // returned, for instance if the connection cost is lower than the heuristic-estimate-based error.
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

      closedList.Add(smallestCostSoFar);
      openList.Remove(smallestCostSoFar);

    }

    for (PathfindingNode waypoint = pathfindingNodes[endNode]; waypoint != null; waypoint = waypoint.predecessor)
    {
      waypoints.Add(waypoint.graphNode);

    }

    waypoints.Reverse();

    return waypoints;
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


}
