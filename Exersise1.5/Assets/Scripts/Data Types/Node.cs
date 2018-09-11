using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Node Does:
 * Container that holds nodeConnections and links to a transform
 */
 [CreateAssetMenu]
public class Node : ScriptableObject
{
  public Dictionary<Node, float> connections = new Dictionary<Node, float>();


  // Steve's alternative
  private int[] connectionState = new int[4];
  private bool[] connectionEnableState = new bool[4];

  private bool CanConnect(int direction)
  {
    return connectionState[direction] == 1 && connectionEnableState[direction];

  }



  public Transform transform;

  // constructor that builds from a incoming transform
  public Node(Transform incomingTransform)
  {
    transform = incomingTransform;

  }

  // displays all connections for a particular node
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
