using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnerController : MonoBehaviour
{
  public bool building;
  public float buildTime;

  public GameObject unitPrefab;

  public PathFindingNode node;

  public void BuildNewUnit()
  {
    StartCoroutine(WaitSpawnUnit());
  }
  
  private IEnumerator WaitSpawnUnit()
  {
    building = true;
    yield return new WaitForSeconds(buildTime);
    building = false;
    SpawnUnit();
  }

  private void SpawnUnit()
  {
    var unit = Instantiate(unitPrefab, transform.position + Vector3.up, Quaternion.identity);
    unit.GetComponent<UnitController>().currentNode = node;
    unit.GetComponent<UnitController>().SetNewPoint(node.connections[0].endNode);
  }
}
