using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
  public Node tileNode;

  public AI ai;

  private void Start()
  {
    ai = GameObject.FindGameObjectWithTag("Player").GetComponent<AI>();

  }
  public void GatherConnections()
  {
    foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
    {
      Vector3 tilePosition = tile.transform.position;

      if (tilePosition == this.transform.position + new Vector3(0, 0, 1) ||
          tilePosition == this.transform.position + new Vector3(0, 0, -1) ||
          tilePosition == this.transform.position + new Vector3(1, 0, 0) ||
          tilePosition == this.transform.position + new Vector3(-1, 0, 0))
      {
        tileNode.connections.Add(tile.GetComponent<TileInfo>().tileNode, 1);

      }
    }
  }

  public void MoveAIToCordinates()
  {
    Debug.Log("New Start Node Set: " + this.transform.name + "\n");
    ai.transform.position = new Vector3(this.transform.position.x, ai.transform.position.y, this.transform.position.z);

  }

  public void SendCordinatesToAI(bool debug)
  {
    if (debug)
    {
      tileNode.DisplayConnections();

    }

    List<Vector3> vectorList;

    try
    {
      vectorList = PathFinder.Dijkstra(ai.startNode, this.tileNode);

      foreach (Vector3 vector3 in vectorList)
      {
        ai.moveQue.Add(vector3);

      }

      ai.startNode = this.tileNode;

    }
    catch
    {
      Debug.LogWarning("Couldn't find a path to: " + this.tileNode.transform.name);

      vectorList = new List<Vector3>();
    }
  }
}
