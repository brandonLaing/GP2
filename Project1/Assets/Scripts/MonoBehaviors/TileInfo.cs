using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
  public Node tileNode;

  public GameObject obstaclePrefab;
  public GameObject towerPrefab;

  public GameObject obstacle;

  public void MoveObjectsToPosition(Transform objectTransform)
  {
    objectTransform.position = new Vector3(this.transform.position.x, objectTransform.position.y, this.transform.position.z);

  }

  public void MakeObstacle(ObstaclePlacer placer)
  {
    if (obstacle == null)
    {
      obstacle = Instantiate(obstaclePrefab, this.transform.position, obstaclePrefab.transform.rotation, this.transform);
      placer.tilesToPlace--;
    }

    else
    {
      Debug.LogWarning("That tile already has a obstacle");

    }
  }

  public void MakeTower()
  {
    obstacle = Instantiate(towerPrefab, this.transform.position, towerPrefab.transform.rotation, this.transform);

  }

  public void RemoveObstacle()
  {
    if (obstacle != null)
    {
      Destroy(obstacle.gameObject);

      obstacle = null;

    }

    else
    {
      Debug.LogWarning("There was nothing to destroy");

    }
  }

}
