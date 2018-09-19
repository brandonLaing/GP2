  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaser : AIPathFinder
{
  [Header("Chaser Specific")]
  public AIRunner runner;

  private Node currentDestination;

  private void Update()
  {
    moveQueDebug = moveQue.Count;
    moveWaypointsDebug = moveWaypoints.Count;

    ProgressOnMoveQue();
    MakeMoveQueTowardsRunner();
  }

  public  override void ProgressOnMoveQue()
  {
    if (moveQue.Count > 0)
    {
      Vector3 tempMove = moveQue[0].nodeTransform.position;

      tempMove.y = this.transform.position.y;

      transform.position = Vector3.MoveTowards(this.transform.position, tempMove, moveSpeed * Time.deltaTime);

      if (Vector3.Distance(this.transform.position, tempMove) < .01F)
      {
        try
        {
          currentNode = moveQue[1];

        }
        catch
        {

        }
        moveQue.Remove(moveQue[0]);

      }
    }

  }

  private void MakeMoveQueTowardsRunner()
  {
    if (currentDestination != runner.currentNode)
    {
      moveQue = new List<Node>();
      moveWaypoints = new List<Node>();

      FindPathTo(runner.currentNode);
      currentDestination = moveQue[moveQue.Count - 1];

    }

  }

}
