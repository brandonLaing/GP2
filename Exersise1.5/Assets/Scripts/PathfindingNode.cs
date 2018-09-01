using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathfindingNode : IComparable<PathfindingNode>
{
  public Node graphNode;

  public float costSoFar;

  public PathfindingNode predecessor;


  public PathfindingNode(Node newNode)
  {
    graphNode = newNode;
    costSoFar = 0;

  }

  public PathfindingNode(Node graphNode, float costSoFar, PathfindingNode predecessor)
  {
    this.graphNode = graphNode;
    this.costSoFar = costSoFar;
    this.predecessor = predecessor;
  }

  public int CompareTo(PathfindingNode other)
  {
    return costSoFar.CompareTo(other.costSoFar);

  }

}
