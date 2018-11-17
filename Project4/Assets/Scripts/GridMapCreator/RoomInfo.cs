using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
  public Vector2Int indexPosition = new Vector2Int();
  public List<RoomConnectionInfo> connections = new List<RoomConnectionInfo>();

  public void SetPosition(Vector2Int indexPosition)
  {
    this.indexPosition = indexPosition;
    this.name = string.Format("<{0}, {1}>", indexPosition.x, indexPosition.y);
    transform.name = string.Format("<{0}, {1}>", indexPosition.x, indexPosition.y);
  }

  public void BuildConnection(GameObject connectedGameObject, bool IsConnected, bool IsAccessable)
  {
    RoomConnectionInfo newConnection = new RoomConnectionInfo(this, connectedGameObject.GetComponent<RoomInfo>(), IsConnected, IsAccessable);
    connections.Add(newConnection);
  }

  public void OpenRoom(RoomInfo roomToOpenTo)
  {
    foreach (RoomConnectionInfo connection in connections)
    {
      if (connection.connectedRoom == roomToOpenTo)
      {
        connection.IsAccessable = true;
      }
    }
  }

  public void OpenRooms()
  {
    foreach (RoomConnectionInfo connectionInfo in connections)
    {
      if (connectionInfo.IsConnected)
      {
        connectionInfo.IsAccessable = true;
        connectionInfo.connectedRoom.OpenRoom(this);
      }
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.black;
    Gizmos.DrawSphere(transform.position + Vector3.up, .2F);

    foreach (RoomConnectionInfo connection in connections)
    {
      if (connection.IsConnected == true && connection.IsAccessable)
      {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position + Vector3.up, connection.connectedRoom.transform.position + Vector3.up);
      }
      else if (connection.IsConnected && !connection.IsAccessable)
      {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position + Vector3.up, connection.connectedRoom.transform.position + Vector3.up);

      }
    }
  }
}

[System.Serializable]
public class RoomConnectionInfo
{
  public RoomInfo room;
  public RoomInfo connectedRoom;
  public bool IsConnected;
  public bool IsAccessable;

  public RoomConnectionInfo(RoomInfo room, RoomInfo connectedRoom, bool IsConnected, bool IsAccessable)
  {
    this.room = room;
    this.connectedRoom = connectedRoom;
    this.IsConnected = IsConnected;
    this.IsAccessable = IsAccessable;
  }
}