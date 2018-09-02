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
        #region Make Tiles
        GameObject newTile = GameObject.Instantiate(tileTemplate, new Vector3(x, 0, z), tileTemplate.transform.rotation, this.gameObject.transform);

        newTile.transform.name += " " + i;

        newTile.GetComponent<TileInfo>().tileNode = new Node(newTile.transform);

        allNodes.Insert(0, newTile.GetComponent<TileInfo>().tileNode);

        i++;
        #endregion
        /**
         * So if its not on any edge then add a connection each way
         */
        #region Check Connections
        int temp = 0;

        foreach (Node nodeChecked in allNodes)
        {
          if ((nodeChecked.transform.position == newTile.transform.position + new Vector3(0, 0, -1) ||
               nodeChecked.transform.position == newTile.transform.position + new Vector3(-1, 0, 0)) && temp < 2)
          {
            temp++;

            newTile.GetComponent<TileInfo>().tileNode.connections.Add(nodeChecked, 1);
            nodeChecked.connections.Add(newTile.GetComponent<TileInfo>().tileNode, 1);

          }
        }
        #endregion

        #region Set Color
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
        #endregion
      }
    }

    int randomNumber = Random.Range(0, allNodes.Count);

    allNodes[randomNumber].transform.GetComponent<TileInfo>().MoveAIToCordinates();

    aiPlayer.startNode = allNodes[randomNumber].transform.GetComponent<TileInfo>().tileNode;

    aiPlayer.allNodes = allNodes;

    Destroy(this);

    Camera.main.transform.position = new Vector3(gridWidth / 2, Camera.main.transform.position.y, gridHeight / 2);

  }
}