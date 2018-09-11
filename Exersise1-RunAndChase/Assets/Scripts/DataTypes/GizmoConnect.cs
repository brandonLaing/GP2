using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoConnect
{
  public Vector3 positionOne;
  public Vector3 positionTwo;
  public Vector3 direction;

  public GizmoConnect(Node nodeOne, Node nodeTwo)
  {
    this.positionOne = nodeOne.nodeTransform.position;
    this.positionTwo = nodeTwo.nodeTransform.position;

    this.positionOne.y = 1;
    this.positionTwo.y = 1;

    direction = this.positionOne - this.positionTwo;

  }
}
