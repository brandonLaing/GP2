using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationItemSpawner : MonoBehaviour {

  public List<GameObject> spawnedObjects = new List<GameObject>();
  public Transform spawnedObjectsContainer;
  public void SpawnObject(GameObject obj)
  {
    GameObject newObj = Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity, spawnedObjectsContainer);
    spawnedObjects.Add(newObj);
  }
}
