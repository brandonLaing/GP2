using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelCreator : MonoBehaviour
{
  [Header("Map settings")]
  public int mapSize;
  public int mapOffset;
  public Vector2Int startingPosition;

  public static int mapStaticSize;

  public int numberOfBasicRooms;

  public int minEmptySpaceClump = 1;
  public int maxEmptySpaceClump = 10;

  private List<RoomData> _emptySpaceClump = new List<RoomData>();

  public GameObject roomPrefab;

  [Header("Background")]
  public float backgroundY;
  public GameObject yellowBackgroundPrefab;
  public GameObject blackBackgroundPrefab;

  public Transform backgroundHolder;
  public Transform roomHolder;

  private RoomData[,] _roomDataGrid;
  private List<RoomData> _buildableRooms;

  private GameObject[,] _gameObjectGrid;
  private List<GameObject> _builtRooms;

  public bool logDebugs;

  private void Start()
  {
    mapStaticSize = mapSize;

    BuildBackground();

    BuildRooms();

    BuildMap();

    DisplayRoomsToText();

  }


  public void BuildRooms()
  {
    _roomDataGrid = new RoomData[mapSize, mapSize];
    _buildableRooms = new List<RoomData>();

    _gameObjectGrid = new GameObject[mapSize, mapSize];
    _builtRooms = new List<GameObject>();

    BuildStartingRoom();

    for (int i = 0; i < numberOfBasicRooms; i++)
    {
      if (!BuildBasicRoom(RoomType.Basic))
        return;
      if (i % 8 == 0)
      {
        if (logDebugs)
          Debug.Log("Building new set of empty rooms");
        if (!BuildWhiteSpace())
          return;
      }
    }
  }

  #region Room Building
  public void BuildStartingRoom()
  {
    RoomData startingRoom = new RoomData(mapSize / 2, mapSize / 2);
    startingRoom.roomType = RoomType.Starting;

    _roomDataGrid[startingRoom.Position.x, startingRoom.Position.y] = startingRoom;
    _buildableRooms.Add(startingRoom);

    if (logDebugs)
      Debug.Log(string.Format("Made room at <{0}, {1}>", startingRoom.Position.x, startingRoom.Position.y));

  }

  public bool BuildBasicRoom(RoomType roomType)
  {
    if (_buildableRooms.Count <= 0)
    {
      Debug.LogWarning("Couldn't build anymore rooms in level");
      return false;
    }
    RoomData baseRoom = null;
    if (roomType != RoomType.Empty || (_emptySpaceClump.Count == 0))
    {
      // grab a random buildable room
      baseRoom = _buildableRooms[Random.Range(0, _buildableRooms.Count)];
    }
    else
    {
      baseRoom = _emptySpaceClump[Random.Range(0, _emptySpaceClump.Count)];
    }

    RoomConnectionData baseConnection = null;

    try
    {
      // get a random connection from that
      baseConnection = baseRoom.RandomOpenConnection;
    }
    catch
    {
      Debug.LogError("Error getting random open connection from " + roomType.ToString() + " room.");
      if (_buildableRooms.Contains(baseRoom))
        Debug.LogError("the base room is in the buildable list");
      else
        Debug.LogError("the base room is not in the buildable list");

    }

    // make a new room from the base room in the direction of the connection
    RoomData addedRoom = new RoomData(baseRoom.Position + baseConnection.Direction)
    {
      roomType = roomType
    };

    if (logDebugs)
      Debug.Log(string.Format("Made new {0} room at <{1}, {2}>", roomType.ToString(), addedRoom.Position.x, addedRoom.Position.y));

    if (_roomDataGrid[addedRoom.Position.x, addedRoom.Position.y] != null)
      Debug.LogWarning("Building new room in ocupied position");

    // add our new room to the buildable rooms and the grid
    if (roomType == RoomType.Basic)
    {
      _buildableRooms.Add(addedRoom);
    }
    else if (roomType == RoomType.Empty)
    {
      _emptySpaceClump.Add(addedRoom);
    }

    _roomDataGrid[addedRoom.Position.x, addedRoom.Position.y] = addedRoom;

    // for every connection in the new room
    foreach (RoomConnectionData connection in addedRoom.connections)
    {
      // grab the position of that connection
      Vector2Int connectionPosition = addedRoom.Position + connection.Direction;

      // if there is a room in that connections position
      if (_roomDataGrid[connectionPosition.x, connectionPosition.y] != null)
      {
        // grab that other room
        RoomData otherRoom = _roomDataGrid[connectionPosition.x, connectionPosition.y];

        // add connection to the base room
        if (otherRoom == baseRoom)
        {
          if (roomType == RoomType.Empty)
          {
            connection.ClosePosition(otherRoom);
            otherRoom.GetConnectionInPosition(addedRoom.Position).ClosePosition(addedRoom);
          }
          else
          {
            connection.AddConnection(otherRoom);
            otherRoom.GetConnectionInPosition(addedRoom.Position).AddConnection(addedRoom);
          }
        }
        // close position for every other one
        else
        {
          // make connection between our current connection and the other room
          connection.ClosePosition(otherRoom);
          otherRoom.GetConnectionInPosition(addedRoom.Position).ClosePosition(addedRoom);
        }

        // check if the other room now has zero open connections
        if (!otherRoom.HasConnectionOpen)
        {
          if (logDebugs)
            Debug.Log(string.Format("Removing room at <{0}, {1}> from buildable rooms", otherRoom.Position.x, otherRoom.Position.y));
          if (_buildableRooms.Contains(otherRoom))
            _buildableRooms.Remove(otherRoom);
          if (_emptySpaceClump.Contains(otherRoom))
            _emptySpaceClump.Remove(otherRoom);
        }
      }
    }

    if (!addedRoom.HasConnectionOpen)
    {
      if (logDebugs)
        Debug.Log(string.Format("Removing room at <{0}, {1}> from buildable rooms", addedRoom.Position.x, addedRoom.Position.y));
      if (_buildableRooms.Contains(addedRoom))
        _buildableRooms.Remove(addedRoom);
      if (_emptySpaceClump.Contains(addedRoom))
        _emptySpaceClump.Remove(addedRoom);
    }

    return true;
  }

  public bool BuildWhiteSpace()
  {
    _emptySpaceClump = new List<RoomData>();

    var whiteSpaceClump = Random.Range(minEmptySpaceClump, maxEmptySpaceClump + 1);
    for (int i = 0; i < whiteSpaceClump; i++)
    {
      if (!BuildBasicRoom(RoomType.Empty))
        return false;
    }

    return true;
  }
  #endregion

  #region Scene Building
  public void BuildBackground()
  {
    for (int i = 0; i < mapSize; i++)
    {
      for (int j = 0; j < mapSize; j++)
      {
        if (i % 2 == 0)
        {
          if (j % 2 == 0)
          {
            Instantiate(yellowBackgroundPrefab, new Vector3(i * mapOffset, backgroundY, j * mapOffset), Quaternion.identity, backgroundHolder);
          }
          else
          {
            Instantiate(blackBackgroundPrefab, new Vector3(i * mapOffset, backgroundY, j * mapOffset), Quaternion.identity, backgroundHolder);
          }
        }
        else
        {
          if (j % 2 == 0)
          {
            Instantiate(blackBackgroundPrefab, new Vector3(i * mapOffset, backgroundY, j * mapOffset), Quaternion.identity, backgroundHolder);
          }
          else
          {
            Instantiate(yellowBackgroundPrefab, new Vector3(i * mapOffset, backgroundY, j * mapOffset), Quaternion.identity, backgroundHolder);
          }
        }
      }
    }
  }

  public void BuildMap()
  {
    for (int i = 0; i < mapSize; i++)
    {
      for (int j = 0; j < mapSize; j++)
      {
        if (_roomDataGrid[i, j] != null)
        {
          GameObject newRoom = Instantiate(roomPrefab, new Vector3(i * mapOffset, 0, j * mapOffset), Quaternion.identity, roomHolder);
          _gameObjectGrid[i, j] = newRoom;
          newRoom.GetComponent<RoomInfo>().SetPosition(new Vector2Int(i, j));
          newRoom.GetComponent<RoomInfo>().roomType = _roomDataGrid[i, j].roomType;
          _builtRooms.Add(newRoom);
        }
      }
    }

    for (int i = 0; i < mapSize; i++)
    {
      for (int j = 0; j < mapSize; j++)
      {
        if (_gameObjectGrid[i, j] != null)
        {
          var currentRoomInfo = _gameObjectGrid[i, j].GetComponent<RoomInfo>();

          foreach (var connection in _roomDataGrid[i, j].connections)
          {
            if (!connection.IsOpen)
            {
              var connectedPos = connection.ConnectedRoom.Position;
              var connectionGameObject = _gameObjectGrid[connectedPos.x, connectedPos.y];
              currentRoomInfo.BuildConnection(connectionGameObject, connection.IsConnected, false);
            }
          }
        }
      }
    }
  }
  #endregion

  #region Debugging
  public void DisplayRoomsToText()
  {
    System.Text.StringBuilder sb = new System.Text.StringBuilder();

    for (int i = mapSize; i >= 0; i--)
    {
      for (int j = 0; j < mapSize + 1; j++)
      {
        #region Outside
        if (i == 0 && j == 0)
        {
          sb.Append("0\t");
        }
        else if (i == 0 && j == 1)
        {
          sb.Append("X\t");
        }
        else if (i == 1 && j == 0)
        {
          sb.Append("Y\t");
        }
        else if (i == 0)
        {
          sb.Append((j - 1) + "\t");
        }
        else if (j == 0)
        {
          sb.Append((i - 1) + "\t");
        }
        #endregion

        #region Actual Grid
        else if (_roomDataGrid[j - 1, i - 1] == null)
        {
          sb.Append("E\t");
        }
        else if (_roomDataGrid[j - 1, i - 1].roomType == RoomType.Starting)
        {
          sb.Append("S\t");
        }
        else if (_roomDataGrid[j - 1, i - 1].roomType == RoomType.Basic)
        {
          sb.Append("B\t");
        }

        #endregion
      }

      sb.AppendLine();
      sb.AppendLine();
    }

    if (logDebugs)
      Debug.Log(sb);
  }
  #endregion
}
