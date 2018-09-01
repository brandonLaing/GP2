using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGeneration : MonoBehaviour
{
  public int gridWidth = 10;
  public int gridHeight = 10;

  public GameObject tileTemplate;

  private void Start()
  {
    Dictionary<Vector3, Node> nodesByPosition = new Dictionary<Vector3, Node>();

    for (int x = 0; x < gridWidth; x++)
    {
      for (int z = 0; z < gridHeight; z++)
      {
        GameObject newTile = GameObject.Instantiate(tileTemplate);
        newTile.transform.position = new Vector3(x, 0, z);

        Node tileNode = new Node(newTile.transform);
        nodesByPosition.Add(tileNode.transform.position, tileNode);

      }
    }

    foreach (Node nodeCheck in nodesByPosition.Values)
    {
      if (nodesByPosition.ContainsKey(nodeCheck.transform.position + Vector3.right))
      {
        nodeCheck.connections.Add(nodesByPosition[nodeCheck.transform.position + Vector3.right], 1);

      }

      if (nodesByPosition.ContainsKey(nodeCheck.transform.position + Vector3.left))
      {
        nodeCheck.connections.Add(nodesByPosition[nodeCheck.transform.position + Vector3.right], 1);

      }

      if (nodesByPosition.ContainsKey(nodeCheck.transform.position + Vector3.up))
      {
        nodeCheck.connections.Add(nodesByPosition[nodeCheck.transform.position + Vector3.right], 1);

      }

      if (nodesByPosition.ContainsKey(nodeCheck.transform.position + Vector3.down))
      {
        nodeCheck.connections.Add(nodesByPosition[nodeCheck.transform.position + Vector3.right], 1);

      }



    }
  }
}
