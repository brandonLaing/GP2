using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingTile : MonoBehaviour
{
  public PathFindingNode tileNode;

  public GameObject building;
  public GameObject towerPrefab;
  public GameObject resoucePrefab;

  public bool buildable;

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

  public delegate void BuildingAction();

  public Dictionary<string, BuildingAction> actions = new Dictionary<string, BuildingAction>();

  public void SetActions(RoomType roomType)
  {
    actions.Add("Build Unit Spawner", BuildUnitSpawner);
  }

  public void BuildUnitSpawner()
  {
    if (building == null && buildable)
    {
      for (int i = 0; i < tileNode.connections.Length; i++)
      {
        if (tileNode.connections[i] == null ||
          tileNode.connections[i].endNode.nodeTransform.GetComponent<PathingTile>().building != null)
        {
          return;
        }
        else
        {
          for (int j = 0; j < tileNode.connections[i].endNode.connections.Length; j++)
          {
            if (tileNode.connections[i].endNode.connections[j] == null ||
              tileNode.connections[i].endNode.connections[j].endNode.nodeTransform.GetComponent<PathingTile>().building != null)
            {
              return;
            }
          }
        }
      }

      building = Instantiate(towerPrefab, transform.position + Vector3.up, Quaternion.identity, this.transform);

      StartCoroutine(WaitThenBlock());
    }
    else
    {
      return;
    }
  }

  public bool BuildResourceBuilding(RoomInfo room, out GameObject newBuilding)
  {
    if (building == null)
    {
      building = Instantiate(resoucePrefab, transform.position + Vector3.up, Quaternion.identity, this.transform);
      building.GetComponent<ResourceBuildingController>().room = room;
      newBuilding = building;
      return true;
    }

    newBuilding = building;
    return false;
  }

  private IEnumerator WaitThenBlock()
  {
    yield return new WaitForEndOfFrame();
    building.GetComponent<UnitSpawnerController>().node = tileNode;
    tileNode.Block();
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
