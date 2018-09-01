using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StevesNode
{
  #region Variables
  public Dictionary<Node, float> connections = new Dictionary<Node, float>();

  public Vector3 position;

  #endregion

  public StevesNode(Vector3 pos)
  {
    this.position = pos;

  }

  //public void DisplayConnections()
  //{
  //  foreach (KeyValuePair<StevesNode, float> connection in connections)
  //  {
  //    Debug.Log("Tile Name: " + connection.Key.transform.name +
  //      "Weight: " + connection.Value);

  //  }
  //}

}
