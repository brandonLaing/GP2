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
      var movePosition = moveQue[0].nodeTransform.position;
      movePosition.y = 1;

      transform.position = Vector3.MoveTowards(transform.position, movePosition, moveSpeed * Time.deltaTime);
      if (Vector3.Distance(transform.position, movePosition) < .1F)
      {
        currentNode = moveQue[0];
        moveQue.Remove(moveQue[0]);
      }
    }
  }

  public void SetStartingPoint(PathFindingNode endNode)
  {
    moveQue = new List<PathFindingNode>()
    {
      endNode
    };
  }

  public void SetNewPoint(PathFindingNode endNode)
  {
    moveQue = PathFinder.DijkstraNodes(currentNode, endNode);
  }

  public void AddNewPoint(PathFindingNode endNode)
  {
    foreach (PathFindingNode node in PathFinder.DijkstraNodes(moveQue[moveQue.Count - 1], endNode))
      moveQue.Add(node);

  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponentInParent<ResourceBuildingController>() != null)
    {
      other.GetComponentInParent<ResourceBuildingController>().SendMessage();
      Destroy(other.gameObject);
    }
  }
}
