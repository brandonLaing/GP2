using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAsMainNode : MonoBehaviour
{
  public AI player;

  private void Start()
  {
    player.startNode = gameObject.GetComponent<TileInfo>().node;
  }
}
