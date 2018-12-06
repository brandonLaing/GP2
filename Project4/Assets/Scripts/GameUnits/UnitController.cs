using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
  public PathFindingNode currentNode;
  public List<PathFindingNode> moveQue = new List<PathFindingNode>();

  public float moveSpeed;

  private void Update()
  {
    if (moveQue.Count > 0)
    {

    }
  }

  public void SetNewPoint(PathFindingNode endNode)
  {

  }

  public void AddNewPoint(PathFindingNode endNode)
  {

  }
}
