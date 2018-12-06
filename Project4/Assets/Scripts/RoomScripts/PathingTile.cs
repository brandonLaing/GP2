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

  public delegate void BuildingAction();

  public Dictionary<string, BuildingAction> actions = new Dictionary<string, BuildingAction>();

  public void SetActions(RoomType roomType)
  {
    actions.Add("Build Unit Spawner", BuildUnitSpawner);
  }

  public void BuildUnitSpawner()
  {
    Debug.Log("Spawing Unit spawner");
    if (building == null)
    {
      for (int i = 0; i < tileNode.connections.Length; i++)
      {
        Debug.Log("going though loop");

        if (tileNode.connections[i] == null ||
          tileNode.connections[i].endNode.nodeTransform.GetComponent<PathingTile>().building != null)
        {
          Debug.Log("couldnt build room");
          return;
        }
        else
        {
          for (int j = 0; j < tileNode.connections[i].endNode.connections.Length; j++)
          {
            if (tileNode.connections[i].endNode.connections[j] == null ||
              tileNode.connections[i].endNode.connections[j].endNode.nodeTransform.GetComponent<PathingTile>().building != null)
            {
              Debug.Log("couldnt build room");
              return;
            }
          }
        }
      }

      building = Instantiate(towerPrefab, transform.position + Vector3.up, Quaternion.identity, this.transform);
      building.GetComponent<UnitSpawnerController>().node = tileNode;
      tileNode.Block();
    }
    else
    {
      Debug.Log("couldnt build room");
      return;
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
