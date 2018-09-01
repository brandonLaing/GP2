using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGeneration2 : MonoBehaviour
{
  public int gridWidth = 10;
  public int gridHeight = 10;

  public GameObject tileTemplate;

  void Start()
  {
    for (int x = 0; x < gridWidth; x++)
    {
      for (int z = 0; z < gridHeight; z++)
      {
        GameObject newTile = GameObject.Instantiate(tileTemplate, new Vector3(x, 0, z), tileTemplate.transform.rotation);

      }
    }
  }
}
