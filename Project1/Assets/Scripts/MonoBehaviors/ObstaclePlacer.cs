using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePlacer : MonoBehaviour
{
  public TileGenerator tileGen;

  public int tilesToPlace;

  void Update ()
  {
    if (Input.GetMouseButtonDown(1))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      RaycastHit hit;

      if (Physics.Raycast(ray, out hit) && tilesToPlace > 0)
      {
        if (hit.transform.tag == "Tile")
        {
          hit.transform.GetComponent<TileInfo>().MakeObstacle(this);
          tileGen.RecheckConnections(); // Here to TileGen 242

        }
      }
    }
	}
}
