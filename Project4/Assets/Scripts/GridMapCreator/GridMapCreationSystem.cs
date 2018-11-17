using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMapCreationSystem : MonoBehaviour
{
  public int levelWidth, levelHeight;
  public static int staticLevelWidth, staticLevelHeight;

  private RoomData[,] _gridDataMap;
  private List<RoomData> buildableRooms = new List<RoomData>();

  public int numberOfRooms;
  public GameObject prefab;

  private List<GameObject> builtObjects = new List<GameObject>();

  private GameObject[,] _gridGameObjectMap;

  public bool logDebugs = false;

  public void Start()
  {
    staticLevelWidth = levelWidth; staticLevelHeight = levelHeight;

    _gridDataMap = new RoomData[levelWidth, levelHeight];

    //BuildStartingRoom();
    ////StartCoroutine(BuildGrid());
    //DisplayRoomText();

    StartCoroutine(CreateMap());
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      BuildNewRoom();
    }

    if (Input.GetKeyDown(KeyCode.Return))
    {
      InstantiateMap();
    }
  }

  public IEnumerator CreateMap()
  {
    yield return new WaitForEndOfFrame();
    BuildStartingRoom();
    yield return new WaitForEndOfFrame();
    
    for (int i  = 1; i < numberOfRooms; i++)
    {
      BuildNewRoom();
      yield return new WaitForEndOfFrame();
    }

    yield return new WaitForEndOfFrame();

    InstantiateMap();

    yield return new WaitForEndOfFrame();

    DisplayRoomText();
  }

  public void BuildStartingRoom()
  {
    RoomData startingRoom = new RoomData(levelWidth / 2, levelHeight / 2);
    _gridDataMap[startingRoom.Position.x, startingRoom.Position.y] = startingRoom;
    buildableRooms.Add(startingRoom);

    if (logDebugs)
      Debug.Log(string.Format("Made starting room at <{0}, {1}>", startingRoom.Position.x, startingRoom.Position.y));
  }

  public void BuildNewRoom()
  {
    if (buildableRooms.Count == 0)
    {
      Debug.LogWarning("Couldn't build anymore rooms in level");

      return;
    }

    #region Build the room
    // get a random buildable room
    RoomData startingRoom = buildableRooms[Random.Range(0, buildableRooms.Count)];
    // get a random connection from that buildable room
    RoomConnectionData startingConnection = startingRoom.RandomOpenConnection;
    // build a new room in the current rooms position plus the connections direction
    RoomData endingRoom = new RoomData(startingRoom.Position + startingConnection.ConnectionDirection);

    // announce this
    if (logDebugs)
      Debug.Log(string.Format("Made new room at <{0}, {1}> from <{2}, {3}>", endingRoom.Position.x, endingRoom.Position.y, startingRoom.Position.x, startingRoom.Position.y));

    if (_gridDataMap[endingRoom.Position.x, endingRoom.Position.y] != null)
    {
      Debug.LogWarning("Building new room where one already exist");
    }

    buildableRooms.Add(endingRoom);
    _gridDataMap[endingRoom.Position.x, endingRoom.Position.y] = endingRoom;
    #endregion

    #region Update other rooms info
    // ok so were going to grab the new room and for every connection were going to check if there is a room in that position using the grid and if there is were
    // going to update this with that info then update that ones info with the info that the new room exist. The only exception is that if its the room be built from
    // we have already set that up

    // set up connections between new room and old room
    startingConnection.AddConnection(endingRoom);
    endingRoom.GetConnectionInPosition(startingRoom.Position).AddConnection(startingRoom);

    foreach (RoomConnectionData endConnection in endingRoom.connections)
    {
      Vector2Int endConnectionRoomPosition = endingRoom.Position + endConnection.ConnectionDirection;

      if (_gridDataMap[endConnectionRoomPosition.x, endConnectionRoomPosition.y] != null)
      {
        RoomData otherRoom = _gridDataMap[endConnectionRoomPosition.x, endConnectionRoomPosition.y];

        if (otherRoom != startingRoom)
        {
          otherRoom.GetConnectionInPosition(endingRoom.Position).ClosePosition(endingRoom);
          endingRoom.GetConnectionInPosition(otherRoom.Position).ClosePosition(otherRoom);
        }

        if (!otherRoom.HasConnectionOpen)
        {
          if (logDebugs)
            Debug.Log(string.Format("Removing room at <{0}, {1}> from buildable rooms", otherRoom.Position.x, otherRoom.Position.y));
          buildableRooms.Remove(otherRoom);
        }
        else
        {
          if (logDebugs)
            Debug.Log(string.Format("Number of build options for <{0}, {1}>: ", otherRoom.Position.x, otherRoom.Position.y) + otherRoom.NumberOfBuildOptions);
        }
      }
    }

    if (!endingRoom.HasConnectionOpen)
    {
      buildableRooms.Remove(endingRoom);
    }

    #endregion

    DisplayRoomText();
  }

  public void DisplayRoomText()
  {
    System.Text.StringBuilder sb = new System.Text.StringBuilder();

    for (int h = levelHeight - 1; h >= 0; h--)
    {
      for (int w = 0; w < levelWidth; w++)
      {
        if (_gridDataMap[w, h] != null)
        {
          sb.Append("R\t");
        }
        else if (w == 0 && h == 0)
        {
          sb.Append("0\t");
        }
        else if (w == 0 && h == 1)
        {
          sb.Append("Y\t");
        }
        else if (w == 1 && h == 0)
        {
          sb.Append("X\t");
        }
        else
        {
          sb.Append("E\t");
        }
      }
      sb.AppendLine();
      sb.AppendLine();
    }

    if (logDebugs)
      Debug.Log(sb);

    #region Bad
    /*
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    sb.AppendLine();

    for (int i = levelHeight - 1; i >= 0; i--)
    {
      for (int j = 0; j < levelWidth; j++)
      {
        if (_gridMap[i, j] != null)
        {
          sb.Append("R\t");
        }
        else if (i == 0 && j == 0)
        {
          sb.Append("0\t");
        }
        else
        {
          sb.Append("E\t");
        }
      }
      sb.AppendLine();
      sb.AppendLine();
    }

    Debug.Log(sb);
    */
    #endregion
  }

  public void InstantiateMap()
  {
    foreach (GameObject obj in builtObjects)
    {
      Destroy(obj);
    }

    builtObjects = new List<GameObject>();
    _gridGameObjectMap = new GameObject[levelWidth, levelHeight];

    for (int i = 0; i < levelWidth; i++)
    {
      for (int j = 0; j < levelHeight; j++)
      {
        if (_gridDataMap[i, j] != null)
        {
          GameObject newRoom = Instantiate(prefab, new Vector3(i, 0, j), Quaternion.identity);
          _gridGameObjectMap[i, j] = newRoom;
          newRoom.GetComponent<RoomInfo>().SetPosition(new Vector2Int(i, j));
          builtObjects.Add(newRoom);
        }
      }
    }

    for (int i = 0; i < levelWidth; i++)
    {
      for (int j = 0; j < levelHeight; j++)
      {
        if (_gridDataMap[i,j] != null)
        {
          RoomInfo currentRoomInfo = _gridGameObjectMap[i, j].GetComponent<RoomInfo>();

          foreach (RoomConnectionData connection in _gridDataMap[i, j].connections)
          {
            if (!connection.IsOpen)
            {
              Vector2Int connectedPos = connection.ConnectedRoom.Position;

              GameObject connectionGameObject = _gridGameObjectMap[connectedPos.x, connectedPos.y];

              if ((i == levelWidth / 2 && j == levelHeight /2) || (connectedPos.x == levelWidth / 2 && connectedPos.y == levelHeight / 2))
              {
                currentRoomInfo.BuildConnection(connectionGameObject, connection.IsConnected, true);
              }
              else
              {
                currentRoomInfo.BuildConnection(connectionGameObject, connection.IsConnected, false);
              }
            }
          }
        }
      }
    }
  }
}

public class RoomData
{
  #region Variables
  /// <summary>
  /// Position on 2D grid for this roomData
  /// </summary>
  private int x, y;

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
    if (Position.y != GridMapCreationSystem.staticLevelHeight - 1)
    {
      connections.Add(new RoomConnectionData(this, new Vector2Int(0, 1)));
    }
    if (Position.x != GridMapCreationSystem.staticLevelWidth - 1)
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
      if ((Position + connection.ConnectionDirection) == roomPosition)
      {
        return connection;
      }
    }

    return null;
  }
  #endregion
}

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
  public Vector2Int ConnectionDirection
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
    this.ConnectionDirection = connectionDirection;

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