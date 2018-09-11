using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaser : MonoBehaviour
{
  public List<Node> allNodes = new List<Node>();
  public List<Node> moveQue = new List<Node>();

  public int moveQueCount;

  public float moveSpeed;

  public float removeDistance;

  public Node currentNode;

  public TileGenerator tileGen;

  public AIRunner runner;

  private void Update()
  {
    moveQueCount = moveQue.Count;

    if (moveQue.Count == 0)
    {
      if (currentNode != null && runner.currentNode != null)
      {
        moveQue = PathFinder.DijkstraNodes(currentNode, runner.currentNode);

      }
    }

    if (moveQue.Count > 0)
    {
      Vector3 tempMove = moveQue[0].transform.position;

      tempMove.y = this.transform.position.y;

      transform.position = Vector3.MoveTowards(this.transform.position, tempMove, moveSpeed * Time.deltaTime);

      if (Vector3.Distance(this.transform.position, tempMove) < removeDistance)
      {
        currentNode = moveQue[1];
        moveQue.Remove(moveQue[0]);

      }
    }
  }

  public void UpdateMoveQue()
  { 
    if (currentNode != null && runner.currentNode != null)
    {
      moveQue = PathFinder.DijkstraNodes(currentNode, runner.currentNode);

    }

  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Cube")
    {
      if (moveQue.Count == 0)
      {
        currentNode = tileGen.PickRandomStartLocationForRunner(this.gameObject);

      }

      else
      {
        this.transform.position = moveQue[0].transform.position + Vector3.up;
      }
    }
  }

}
