using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
  #region Variables
  private int x, y;

  /// <summary>
  /// Position on 2D grid for this roomData
  /// </summary>
  public Vector2Int Position
  {
    get
    {
      return new Vector2Int(x, y);
    }
    set
    {
      x = value.x;
      y = value.y;
    }
  }

  /// <summary>
  /// Connections for this room
  /// </summary>
  public List<RoomConnectionData> connections = new List<RoomConnectionData>();

  private int typeInt;
  public RoomType roomType
  {
    get
    {
      return (RoomType)typeInt;
    }
    set
    {
      typeInt = (int)value;
    }
  }
  #endregion

  #region Constructors
  public RoomData(Vector2Int roomPosition)
  {
    this.Position = roomPosition;
    BuildRoomBaseConnections();
  }

  public RoomData(int roomX, int roomY)
  {
    this.Position = new Vector2Int(roomX, roomY);
    BuildRoomBaseConnections();
  }
  #endregion

  #region Properties
  /// <summary>
  /// Checks if the room has a connection it can build in
  /// </summary>
  public bool HasConnectionOpen
  {
    get
    {
      return (NumberOfBuildOptions != 0);
    }
  }

  /// <summary>
  /// Checks for the number of rooms it can build in
  /// </summary>
  public int NumberOfBuildOptions
  {
    get
    {
      int temp = 0;
      foreach (RoomConnectionData connection in connections)
      {
        if (connection.IsOpen)
          temp++;
      }
      return temp;
    }
  }

  /// <summary>
  /// Returns list of all open rooms for this connection
  /// </summary>
  public List<RoomConnectionData> OpenRooms
  {
    get
    {
      List<RoomConnectionData> openList = new List<RoomConnectionData>();

      foreach (RoomConnectionData connection in connections)
      {
        if (connection.IsOpen)
          openList.Add(connection);
      }

      return openList;
    }
  }

  /// <summary>
  /// Returns a random open room
  /// </summary>
  public RoomConnectionData RandomOpenConnection
  {
    get
    {
      return OpenRooms[Random.Range(0, OpenRooms.Count)];
    }
  }
  #endregion

  #region Methods
  /// <summary>
  /// Sets up base empty room connections for a room
  /// </summary>
  private void BuildRoomBaseConnections()
  {
    if (Position.x != 0)
    {
      connections.Add(new RoomConnectionData(this, new Vector2Int(-1, 0)));
    }
    if (Position.y != GameLevelCreator.mapStaticSize - 1)
    {
      connections.Add(new RoomConnectionData(this, new Vector2Int(0, 1)));
    }
    if (Position.x != GameLevelCreator.mapStaticSize - 1)
    {
      connections.Add(new RoomConnectionData(this, new Vector2Int(1, 0)));
    }
    if (Position.y != 0)
    {
      connections.Add(new RoomConnectionData(this, new Vector2Int(0, -1)));
    }
  }

  public RoomConnectionData GetConnectionInPosition(Vector2Int roomPosition)
  {
    foreach (RoomConnectionData connection in connections)
    {
      if ((Position + connection.Direction) == roomPosition)
      {
        return connection;
      }
    }

    return null;
  }
  #endregion
}