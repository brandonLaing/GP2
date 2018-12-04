using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnectionData
{
  #region Variables
  /// <summary>
  /// Holds reference to the connections room
  /// </summary>
  private RoomData _room;

  public RoomData Room
  {
    get
    {
      return _room;
    }
    set
    {
      _room = value;
    }
  }

  private int xDir, yDir;

  /// <summary>
  /// Holds direction the connection is heading
  /// </summary>
  public Vector2Int Direction
  {
    get
    {
      return new Vector2Int(xDir, yDir);
    }
    set
    {
      xDir = value.x;
      yDir = value.y;
    }
  }

  /// <summary>
  /// Holds reference to connected room
  /// </summary>
  public RoomData _connectedRoom;

  public RoomData ConnectedRoom
  {
    get
    {
      return _connectedRoom;
    }
    set
    {
      _connectedRoom = value;
    }
  }

  /// <summary>
  /// Checks if the position in the target direction is open
  /// </summary>
  public bool IsOpen = true;

  /// <summary>
  /// Checks if the room is connected to the connected room
  /// </summary>
  public bool IsConnected = false;
  #endregion

  #region Constructor
  public RoomConnectionData(RoomData room, Vector2Int connectionDirection)
  {
    // set the current room
    this.Room = room;
    // set the direction
    this.Direction = connectionDirection;

    // sets 
    IsOpen = true;
    IsConnected = false;
  }
  #endregion

  #region Methods
  public void AddConnection(RoomData connectedRoom)
  {
    this.ConnectedRoom = connectedRoom;
    IsOpen = false;
    IsConnected = true;
  }

  public void ClosePosition(RoomData connectedRoom)
  {
    this.ConnectedRoom = connectedRoom;
    IsOpen = false;
    IsConnected = false;
  }
  #endregion
}