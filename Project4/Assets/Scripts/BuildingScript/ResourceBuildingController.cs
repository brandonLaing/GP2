using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuildingController : MonoBehaviour
{
  public RoomInfo room;

  public void SendMessage()
  {
    room.CheckToOpen(this.gameObject);
  }
}
