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

  public GameObject yellowPrefab;
  public GameObject blackPrefab;

  public Transform backgroundHolder;

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

  public IEnumerator BuildBackground()
  {
    for (int i = 0; i < levelWidth; i++)
    {
      for (int j = 0; j < levelHeight; j++)
      {
        if (i % 2 == 0)
        {
          if (j % 2 == 0)
          {
            Instantiate(yellowPrefab, new Vector3(i * 14, -1F, j * 14), Quaternion.identity, backgroundHolder);
          }
          else
          {
            Instantiate(blackPrefab, new Vector3(i * 14, -1F, j * 14), Quaternion.identity, backgroundHolder);
          }
        }
        else
        {
          if (j % 2 == 0)
          {
            Instantiate(blackPrefab, new Vector3(i * 14, -1F, j * 14), Quaternion.identity, backgroundHolder);
          }
          else
          {
            Instantiate(yellowPrefab, new Vector3(i * 14, -1F, j * 14), Quaternion.identity, backgroundHolder);
          }
        }

      }
        yield return new WaitForEndOfFrame();
    }
  }

  public IEnumerator CreateMap()
  {
    StartCoroutine(BuildBackground());

    yield return new WaitForEndOfFrame();
    BuildStartingRoom();
    yield return new WaitForEndOfFrame();
    
    for (int i  = 1; i < numberOfRooms; i++)
    {
      BuildNewRoom();

      if (i % 1000 == 0)
      {
        yield return new WaitForEndOfFrame();

        if (logDebugs)
          Debug.Log("Building room " + i);
      }
    }

    yield return new WaitForEndOfFrame();

    StartCoroutine(InstantiateMap());

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
    RoomData baseRoom = buildableRooms[Random.Range(0, buildableRooms.Count)];
    // get a random connection from that buildable room
    RoomConnectionData baseConnection = baseRoom.RandomOpenConnection;
    // build a new room in the current rooms position plus the connections direction
    RoomData addedRoom = new RoomData(baseRoom.Position + baseConnection.Direction);

    Debug.Log(string.Format("Made new room at <{0}, {1}>", addedRoom.Position.x, addedRoom.Position.y));

    // announce this
    if (logDebugs)
      Debug.Log(string.Format("Made new room at <{0}, {1}> from <{2}, {3}>", addedRoom.Position.x, addedRoom.Position.y, baseRoom.Position.x, baseRoom.Position.y));

    if (_gridDataMap[addedRoom.Position.x, addedRoom.Position.y] != null)
    {
      Debug.LogWarning("Building new room where one already exist");
    }

    buildableRooms.Add(addedRoom);
    _gridDataMap[addedRoom.Position.x, addedRoom.Position.y] = addedRoom;
    #endregion

    #region Update other rooms info
    // ok so were going to grab the new room and for every connection were going to check if there is a room in that position using the grid and if there is were
    // going to update this with that info then update that ones info with the info that the new room exist. The only exception is that if its the room be built from
    // we have already set that up

    // set up connections between new room and old room
    baseConnection.AddConnection(addedRoom);
    addedRoom.GetConnectionInPosition(baseRoom.Position).AddConnection(baseRoom);

    foreach (RoomConnectionData connection in addedRoom.connections)
    {
      Vector2Int connectionPosition = addedRoom.Position + connection.Direction;

      if (_gridDataMap[connectionPosition.x, connectionPosition.y] != null)
      {
        RoomData otherRoom = _gridDataMap[connectionPosition.x, connectionPosition.y];

        if (otherRoom != baseRoom)
        {
          otherRoom.GetConnectionInPosition(addedRoom.Position).ClosePosition(addedRoom);
          addedRoom.GetConnectionInPosition(otherRoom.Position).ClosePosition(otherRoom);
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

    if (!addedRoom.HasConnectionOpen)
    {
      buildableRooms.Remove(addedRoom);
    }
    #endregion
  }

  public void DisplayRoomText()
  {
    System.Text.StringBuilder sb = new System.Text.StringBuilder();

    for (int i = levelHeight - 1; i >= 0; i--)
    {
      for (int j = 0; j < levelWidth; j++)
      {
        if (_gridDataMap[j, i] != null)
        {
          sb.Append("R\t");
        }
        else if (j == 0 && i == 0)
        {
          sb.Append("0\t");
        }
        else if (j == 0 && i == 1)
        {
          sb.Append("Y\t");
        }
        else if (j == 1 && i == 0)
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
  }

  public IEnumerator InstantiateMap()
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
          GameObject newRoom = Instantiate(prefab, new Vector3(i * 14, 0, j * 14), Quaternion.identity, this.transform);
          _gridGameObjectMap[i, j] = newRoom;
          newRoom.GetComponent<RoomInfo>().SetPosition(new Vector2Int(i, j));
          builtObjects.Add(newRoom);
        }
      }
      yield return new WaitForEndOfFrame();
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