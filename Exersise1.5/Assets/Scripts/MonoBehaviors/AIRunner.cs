using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRunner : MonoBehaviour
{
  public Node currentNode;
  public List<Node> moveQue = new List<Node>();

  public List<Node> allNodes = new List<Node>();

  public int moveSpeed = 10;

  public TileGenerator tileGen;

  public GameObject chaser;

  public float removeDistance = .1F;

  public float runDistance = 5F;

  private void Update()
  {
    #region Movement and removal of que
    if (moveQue.Count > 0)
    {
      Vector3 tempMove = moveQue[0].transform.position;

      tempMove.y = this.transform.position.y;

      transform.position = Vector3.MoveTowards(this.transform.position, tempMove, moveSpeed * Time.deltaTime);

      if (Vector3.Distance(this.transform.position, tempMove) < removeDistance)
      {
        currentNode = moveQue[0];
        chaser.GetComponent<AIChaser>().UpdateMoveQue();
        moveQue.Remove(moveQue[0]);

      }
    }
    #endregion

    if (Vector3.Distance(this.transform.position, chaser.transform.position) < runDistance)
    {
      Debug.Log("finding next Node");

      PathFinder.FindNextFurthestNode(currentNode, this.gameObject, chaser);

      //FindNextPosition();
    }
  }

  public void FindNextPosition()
  {
    if (moveQue.Count == 0)
    {
      float furthestDistance = Vector3.Distance(this.transform.position, chaser.transform.position);

      Node furtherst = currentNode;

      FindShortestLength(currentNode.connections, ref furtherst, ref furthestDistance);

    }
  }

  public void FindShortestLength(Dictionary<Node, float> connections, ref Node furthest, ref float furthestDistance)
  {
    foreach(KeyValuePair<Node,float> connection in currentNode.connections)
    {
      if (Vector3.Distance(connection.Key.transform.position + Vector3.up, chaser.transform.position) > furthestDistance && connection.Value != 0)
      {
        furthest = connection.Key;
        furthestDistance = Vector3.Distance(connection.Key.transform.position + Vector3.up, chaser.transform.position);

      }
    }

    if (furthest != currentNode)
    {
      moveQue = PathFinder.DijkstraNodes(currentNode, furthest);

    }

    else
    {
      foreach (Node node in connections.Keys)
      {
        FindShortestLength(node.connections, ref furthest, ref furthestDistance);

      }
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

/**
 *       //if (moveQue.Count == 0)
      //{
        //float furthestDistance = Vector3.Distance(this.transform.position, chaser.transform.position);
        //Node furthest = currentNode;

        //foreach (KeyValuePair<Node, float> connection in currentNode.connections)
        //{
        //  if (Vector3.Distance(connection.Key.transform.position + Vector3.up, chaser.transform.position) > furthestDistance && connection.Value != 0)
        //  {
        //    furthest = connection.Key;
        //  }
        //}

        //if (furthest != currentNode)
        //{
        //  moveQue.Add(furthest);

        //}

        //else
        //{
        //  foreach (Node connectionNode in currentNode.connections.Keys)
        //  {
        //    foreach (KeyValuePair<Node, float> connection in connectionNode.connections)
        //    {
        //      if (Vector3.Distance(connection.Key.transform.position + Vector3.up, chaser.transform.position) > furthestDistance && connection.Value != 0)
        //      {
        //        furthest = connection.Key;

        //      }
        //    }

        //  }

        //}

      //}
*/