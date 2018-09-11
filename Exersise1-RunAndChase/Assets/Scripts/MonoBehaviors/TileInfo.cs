using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
  public Node tileNode;

  public void MoveObjectsToPosition(Transform objectTransform)
  {
    objectTransform.position = new Vector3(this.transform.position.x, objectTransform.position.y, this.transform.position.z);

  }
}
