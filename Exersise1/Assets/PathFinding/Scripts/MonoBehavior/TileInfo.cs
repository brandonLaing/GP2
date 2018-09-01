using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
  public Node node = new Node();
  public Transform moveLocation;

  public AI ai;

  public bool check;
  
  private void Start()
  {
    ai = GameObject.FindGameObjectWithTag("Player").GetComponent<AI>();
    node.transform = this.gameObject.transform;
  }

  private void Awake()
  {
    //node.connections.Add(node, 0);

    foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
    {
      Vector3 tilePosition = tile.transform.position;

      if (tilePosition == this.transform.position + new Vector3(0, 0, 1) ||
          tilePosition == this.transform.position + new Vector3(0, 0, -1) ||
          tilePosition == this.transform.position + new Vector3(1, 0, 0) ||
          tilePosition == this.transform.position + new Vector3(-1, 0, 0))
      {
        node.connections.Add(tile.GetComponent<TileInfo>().node , 1);

      }
    }
  }

  private void Update()
  {
    if (node != null)
    {
      check = true;

    }

    else { check = false; }
  }

  public void MoveAIToCordinates()
  {
    ai.transform.position = moveLocation.position;
  }

  public void SendCordinatesToAI()
  {
    ai.moveToQue = ShortestPathFinder.Dijkstra(ai.startNode, this.node);

    ai.startNode = this.node;

    node.DisplayConnections();

  }

}