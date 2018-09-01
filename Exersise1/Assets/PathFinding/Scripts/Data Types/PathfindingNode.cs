using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode : IComparable<PathfindingNode>
{
  public Node graphNode;
  public float costSoFar;

  public PathfindingNode predecessor;


  public int CompareTo(PathfindingNode other)
  {
    return costSoFar.CompareTo(other.costSoFar);

  }

  public PathfindingNode(Node location)
  {
    this.graphNode = location;
    costSoFar = 0;
    predecessor = null;

  }

  public PathfindingNode(Node graphNode, float costSoFar, PathfindingNode predecessor)
  {
    this.graphNode = graphNode;
    this.costSoFar = costSoFar;
    this.predecessor = predecessor;
  }
}

