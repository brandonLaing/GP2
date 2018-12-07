using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingNode
{
  public PathfindingNodeConnection[] connections = new PathfindingNodeConnection[4];
  public Transform nodeTransform;

  public PathFindingNode (Transform nodeTransform)
  {
    this.nodeTransform = nodeTransform;
  }

  // displays each connection
  public void DisplayConnections()
  {
    System.Text.StringBuilder sb = new System.Text.StringBuilder();

    sb.Append("Displaying Connections for: " + this.nodeTransform.name + "\n");

    foreach (PathfindingNodeConnection connection in connections)
    {
      sb.Append("Tile Name: " + connection.endNode.nodeTransform.name +
        "\tWeight: " + connection.cost + "\nConnected: " + connection.IsConnected);
    }
    Debug.Log(sb);
  }

  public void Block()
  {
    for (int i = 0; i < connections.Length; i++)
    {
      if (connections[i] != null)
      {
        connections[i].IsConnected = false;
        connections[i].endNode.FindConnection(this).IsConnected = false;

        // update the tile generator
        nodeTransform.GetComponentInParent<RoomInfo>().UpdateGrizmoConnection(connections[i]);
      }
    }

    //Debug.Log(nodeTransform.name + " was blocked");
  }

  public void Unblock()
  {
    // go though ever connection
    for (int i = 0; i < connections.Length; i++)
    {
      if (connections[i] != null)
      {
        // set this status to that connection to true
        connections[i].IsConnected = true;
        // get that connections connection to this and set its status to true
        connections[i].endNode.FindConnection(this).IsConnected = true;
        // update the tile generations gizmo to reflect that connection is now true

        nodeTransform.GetComponentInParent<RoomInfo>().UpdateGrizmoConnection(connections[i]);
      }
    }

    // send message to log that this tile is blocked
    //Debug.Log(this.nodeTransform.name + " was unblocked");
  }

  public PathfindingNodeConnection FindConnection(PathFindingNode node)
  {
    // go though each connections
    for (int i = 0; i < connections.Length; i++)
    {
      // check if current connections node equals the node were looking for
      if (connections[i].endNode == node)
      {
        // if it is return that node back
        return connections[i];
      }
    }

    // if we don't find our correct node thats some shit so we better send an error message
    Debug.LogError("Tried to find connection for node that isn't in list of connections\n" + nodeTransform.name + " and " + node.nodeTransform.name);
    return null;
  }

  public void AddConnection(PathFindingNode node, bool connectionStatus = true)
  {
    // the node isn't already connected
    if (!IsNodeConnected(node))
    {
      //Debug.Log("Adding connection to " + this.name);
      // go through ever position
      for (int i = 0; i < connections.Length; i++)
      {
        // if the current position is empty
        if (connections[i] == null)
        {
          // make a connection for that node in this position
          //NodeConnection connection = ScriptableObject.CreateInstance<NodeConnection>();
          //connection.NodeConnectionInit(node, connectionStatus);
          //nodeConnections[i] = connection;
          connections[i] = new PathfindingNodeConnection(this, node, 1,connectionStatus);
          return;
        }
      }
    }
  }

  public void ClearConnections()
  {
    connections = new PathfindingNodeConnection[4];
  }

  public bool IsNodeConnected(PathFindingNode node)
  {
    for (int i = 0; i < connections.Length; i++)
    {
      if (connections[i] != null)
      {
        if (connections[i].endNode == node)
        {
          return true;
        }
      }
    }
    return false;
  }

}

public class PathfindingNodeConnection
{
  public PathFindingNode startNode;
  public PathFindingNode endNode;
  public int cost;
  public bool IsConnected; 

  public PathfindingNodeConnection (PathFindingNode startNode, PathFindingNode endNode, int cost = 1, bool connected = true)
  {
    this.startNode = startNode;
    this.endNode = endNode;
    this.cost = cost;
    this.IsConnected = connected;
  }
}
