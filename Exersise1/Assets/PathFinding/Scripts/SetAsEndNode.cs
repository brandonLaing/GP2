using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAsEndNode : MonoBehaviour
{
  public AI player;

  private void Start()
  {
    player.endNode = gameObject.GetComponent<TileInfo>().node;
  }

}
