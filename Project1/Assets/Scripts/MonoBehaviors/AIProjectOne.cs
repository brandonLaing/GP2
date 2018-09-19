using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIProjectOne : AIPathFinder
{
  public Node endNode;

  public TileGenerator tileGen;

  public RoundManager roundMng;

  public int energy = 20;

  private void Update()
  {
    ProgressOnMoveQue();

    CheckIfOutOfEnergy();
  }

  public override void ProgressOnMoveQue()
  {
    if (moveQue.Count > 0)
    {
      Vector3 tempMove = moveQue[0].nodeTransform.position;
      tempMove.y = this.transform.position.y;

      transform.position = Vector3.MoveTowards(this.transform.position, tempMove, moveSpeed * Time.deltaTime);

      if (Vector3.Distance(transform.position, tempMove) < .1F)
      {
        try
        {
          currentNode = moveQue[1];

        }
        catch
        {
          currentNode = moveQue[0];

        }

        moveQue.Remove(moveQue[0]);
        energy--;

      }
    }

    if (moveQue.Count == 0)
    {
      Debug.LogError("OH SHIT");

    }
  }

  public override void RebuildPath()
  {
    moveQue = PathFinder.DijkstraNodes(currentNode, endNode);

  }
  public void PathFindToEnd()
  {
    try
    {
      List<Node> nodeList;

      nodeList = PathFinder.DijkstraNodes(currentNode, endNode);

      foreach (Node nodeInList in nodeList)
      {
        moveQue.Add(nodeInList);

      }
    }
    catch
    {
      if (MouseClicks.doDebug)
      {
        Debug.LogWarning("Couldn't find path to " + endNode.nodeTransform.name + "\n");

      }
    }
  }

  public void CheckIfOutOfEnergy()
  {
    if (energy <= 0)
    {
      DestroyAI();
    }
  }

  private void DestroyAI()
  {
    roundMng.allAIOnBoard.Remove(this);

    Destroy(this.gameObject);

  }

  public override void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Cube")
    {
      // destroy cube but take health way
      energy -= 8;
      if (energy <= 0)
      {
        DestroyAI();

      }
      else
      {
        Destroy(other.gameObject);
        tileGen.RecheckConnections();
        roundMng.RebuildRoots();

      }

    }

    if (other.tag == "Tower")
    {
      other.GetComponent<TowerController>().TakeDamage(energy - 1);
      DestroyAI();

    }
  }
}
