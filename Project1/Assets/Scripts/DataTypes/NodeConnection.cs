using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnection
{
  public Node node;
  public int cost;
  public bool connected;

  public NodeConnection(Node node, int cost, bool connected)
  {
    this.node = node;
    this.cost = cost;
    this.connected = connected;

  }
}
