using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
  #region Variables
  public Dictionary<Node, float> connections = new Dictionary<Node, float>();

  public Transform transform;

  #endregion

  public Node(Transform transform)
  {
    this.transform = transform;

  }

  public Node()
  {

  }

  public void DisplayConnections()
  {
    foreach (KeyValuePair<Node, float> connection in connections)
    {
      Debug.Log("Tile Name: " + connection.Key.transform.name +
        "Weight: " + connection.Value);

    }
  }
}
