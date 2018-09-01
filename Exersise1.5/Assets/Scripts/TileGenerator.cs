using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
  public int gridWidth = 10;
  public int gridHeight = 10;

  public static AI aiPlayer;

  public GameObject tileTemplate;

  public List<Node> allNodes = new List<Node>();

  public Material black;
  public Material white;

  void Start()
  {
    int i = 0;
    for (int x = 0; x < gridWidth; x++)
    {
      for (int z = 0; z < gridHeight; z++)
      {
        GameObject newTile = GameObject.Instantiate(tileTemplate, new Vector3(x, 0, z), tileTemplate.transform.rotation, this.gameObject.transform);

        newTile.transform.name += " " + i;

        newTile.GetComponent<TileInfo>().tileNode = new Node(newTile.transform);

        allNodes.Add(newTile.GetComponent<TileInfo>().tileNode);

        i++;


        if (x % 2 == 0)
        {
          if (z % 2 == 0)
          {
            newTile.GetComponent<MeshRenderer>().material = black;

          }
          else
          {
            newTile.GetComponent<MeshRenderer>().material = white;

          }
        }

        else
        {
          if (z % 2 == 0)
          {
            newTile.GetComponent<MeshRenderer>().material = white;

          }
          else
          {
            newTile.GetComponent<MeshRenderer>().material = black;

          }
        }
      }
    }

    foreach(Node baseNode in allNodes)
    {
      foreach (Node compairingNode in allNodes)
      {

        if (compairingNode.transform.position == baseNode.transform.position + new Vector3(0, 0, 1) ||
            compairingNode.transform.position == baseNode.transform.position + new Vector3(0, 0, -1) ||
            compairingNode.transform.position == baseNode.transform.position + new Vector3(1, 0, 0) ||
            compairingNode.transform.position == baseNode.transform.position + new Vector3(-1, 0, 0))
        {
          baseNode.connections.Add(compairingNode, 1);

        }
      }
    }

    int randomNumber = Random.Range(0, allNodes.Count);

    allNodes[randomNumber].transform.GetComponent<TileInfo>().MoveAIToCordinates();

    aiPlayer.startNode = allNodes[randomNumber].transform.GetComponent<TileInfo>().tileNode;

    aiPlayer.allNodes = allNodes;

    Destroy(this);

  }
}
