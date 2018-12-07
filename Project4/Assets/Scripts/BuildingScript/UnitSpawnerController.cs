using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnerController : MonoBehaviour
{
  public bool building;
  public float buildTime;

  public Material basicMat;
  public Material buildingMat;

  public GameObject unitPrefab;

  public PathFindingNode node;

  public Transform spawnPoint;
  public MeshRenderer rend;

  private void Start()
  {
    rend.material = basicMat;
  }

  public void BuildNewUnit()
  {
    StartCoroutine(WaitSpawnUnit());
  }
  
  private IEnumerator WaitSpawnUnit()
  {
    building = true;
    rend.material = buildingMat;
    yield return new WaitForSeconds(buildTime);
    rend.material = basicMat;
    building = false;
    SpawnUnit();
  }

  private void SpawnUnit()
  {
    var unit = Instantiate(unitPrefab, transform.position, Quaternion.identity);
    unit.GetComponent<UnitController>().currentNode = node;
    unit.GetComponent<UnitController>().SetStartingPoint(node.connections[0].endNode);
  }
}
