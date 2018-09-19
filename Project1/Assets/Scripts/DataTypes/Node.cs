using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
public class Node /*: ScriptableObject */
{
  public List<NodeConnection> connections = new List<NodeConnection>();

  public Transform nodeTransform;

  // constructor sets new transform to transform
  public Node(Transform newTransform)
  {
    nodeTransform = newTransform;

  }

  // displays each connection
  public void DisplayConnections()
  {
    StringBuilder sb = new StringBuilder();

    sb.Append("Displaying Connections for: " + this.nodeTransform.name + "\n");

    foreach (NodeConnection connection in connections)
    {
      sb.Append("Tile Name: " + connection.node.nodeTransform.name +
        "\tWeight: " + connection.cost + "\nConnected: " + connection.connected);

    }

    Debug.Log(sb);

  }
}
