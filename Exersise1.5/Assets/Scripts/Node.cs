using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
  public Dictionary<Node, float> connections = new Dictionary<Node, float>();

  public Transform transform;

  public Node(Transform incomingTransform)
  {
    transform = incomingTransform;
  }
  public void DisplayConnections()
  {
    StringBuilder sb = new StringBuilder();

    sb.Append("Displaying Connections for: " + this.transform.name + "\n");


    foreach (KeyValuePair<Node, float> connection in connections)
    {

      sb.Append("Tile Name: " + connection.Key.transform.name +
        "\tWeight: " + connection.Value + "\n");


    }

    Debug.Log(sb);

  }
}
