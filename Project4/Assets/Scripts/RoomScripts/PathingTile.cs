using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingTile : MonoBehaviour
{
  public PathFindingNode tileNode;

  public GameObject building;
  public GameObject towerPrefab;

  public void MoveTo(Transform targetObject)
  {
    targetObject.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
  }

  public void BuildBuilding(GameObject gameObject)
  {
    if (building == null)
    {
      building = Instantiate(gameObject, this.transform.position, Quaternion.identity, this.transform);
    }
    else
    {
      Debug.LogWarning("Can't place building where one already exist");
    }
  }

  public void DestroyBuilding()
  {
    if (building != null)
    {
      Destroy(building);
    }
    else
    {
      Debug.LogWarning("Can't destroy building where one doesn't exist");
    }
  }
}
