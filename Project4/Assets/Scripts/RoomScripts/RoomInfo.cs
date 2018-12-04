using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
  public Vector2Int indexPosition = new Vector2Int();
  public Dictionary<RoomConnectionInfo, GameObject> connections = new Dictionary<RoomConnectionInfo, GameObject>();

  public bool drawGizmos = true;

  public GameObject greyTilePrefab;
  public GameObject whiteTilePrefab;

  public float roomSize; 

  public Transform MainRoomHolder;
  public Transform wallsHolders;

  public RoomType roomType;

  public GameObject wallPrefab;
  public GameObject doorPrefab;

  public PathFindingNode[] allNodes;

  public List<GizmoConnections> gizmoConnections = new List<GizmoConnections>(), missConnections = new List<GizmoConnections>();
  public bool drawConnectionGizmos;

  private void Start()
  {
    if (roomType != RoomType.Empty)
    {
      BuildFloor();
      BuildStartWalls();
      StartCoroutine(PlaceInbetweenTiles());
    }

    //if (roomType == RoomType.Starting)
    //{
      StartCoroutine(OpenRooms());
    //}
  }

  #region FloorBuilding
  public void BuildFloor()
  {
    List<PathFindingNode> nodeList = new List<PathFindingNode>();

    Vector3 startPosition = transform.position;
    startPosition.x -= 5;
    startPosition.z -= 5;

    GameObject previousRow = null;

    for (int i = 0; i < roomSize; i++)
    {
      var currentRow = new GameObject();
      currentRow.transform.parent = MainRoomHolder;
      currentRow.name = "Row: " + i;

      for (int j = 0; j < roomSize; j++)
      {
        GameObject tile;

        #region Make Tile
        if (i % 2 == 0)
        {
          if (j % 2 == 0)
          {
            tile = PlaceTile(i, j, greyTilePrefab, startPosition, currentRow.transform);
          }
          else
          {
            tile = PlaceTile(i, j, whiteTilePrefab, startPosition, currentRow.transform);
          }
        }

        else
        { 
          if (j % 2 == 0)
          {
            tile = PlaceTile(i, j, whiteTilePrefab, startPosition, currentRow.transform);
          }
          else
          {
            tile = PlaceTile(i, j, greyTilePrefab, startPosition, currentRow.transform);
          }
        }

        nodeList.Insert(0, tile.GetComponent<PathingTile>().tileNode = new PathFindingNode(tile.transform));
        #endregion

        #region Set Connections
        int xPos, zPos;
        GetTilePosFromName(tile.name, out xPos, out zPos);

        if (zPos > 0)
        {
          // find the node
          PathFindingNode thisRowNode = currentRow.transform.Find(xPos + " : " + (zPos - 1)).GetComponent<PathingTile>().tileNode;

          //add connection to the two node
          tile.GetComponent<PathingTile>().tileNode.AddConnection(thisRowNode);
          thisRowNode.AddConnection(tile.GetComponent<PathingTile>().tileNode);

          //// add connection to gizmo
          gizmoConnections.Add(new GizmoConnections(tile.GetComponent<PathingTile>().tileNode, thisRowNode, true));
        }

        // if this isnt in the first row try to connect to the row behind it
        if (xPos > 0)
        {
          // find the node
          PathFindingNode prevRowNode = previousRow.transform.Find((xPos - 1) + " : " + zPos).GetComponent<PathingTile>().tileNode;

          // add connection to the two node
          tile.GetComponent<PathingTile>().tileNode.AddConnection(prevRowNode);
          prevRowNode.AddConnection(tile.GetComponent<PathingTile>().tileNode);

          // add connection to gizmo
          gizmoConnections.Add(new GizmoConnections(tile.GetComponent<PathingTile>().tileNode, prevRowNode, true));
        }
        #endregion
      }

      previousRow = currentRow;
    }
  }

  private void GetTilePosFromName(string name, out int xPos, out int zPos)
  {
    var nameArray = name.Split(':');

    int.TryParse(nameArray[0], out xPos);
    int.TryParse(nameArray[1], out zPos);
  }

  public GameObject PlaceTile(float xPos, float zPos, GameObject tileType, Vector3 startPosition, Transform parent)
  {
    GameObject newTile = Instantiate(tileType, new Vector3(xPos, 0, zPos) + startPosition, Quaternion.identity, parent);
    newTile.name = xPos + " : " + zPos;

    return newTile;
  }

  #endregion

  public void SetPosition(Vector2Int indexPosition)
  {
    this.indexPosition = indexPosition;
    this.name = string.Format("<{0}, {1}>", indexPosition.x, indexPosition.y);
    transform.name = string.Format("<{0}, {1}>", indexPosition.x, indexPosition.y);
  }

  public void BuildConnection(GameObject connectedGameObject, bool IsConnected, bool IsAccessable)
  {
    RoomConnectionInfo newConnection = new RoomConnectionInfo(this, connectedGameObject.GetComponent<RoomInfo>(), IsConnected, IsAccessable);
    connections.Add(newConnection, null);
  }

  public void OpenRoom(RoomInfo roomToOpenTo)
  {
    foreach (RoomConnectionInfo connection in connections.Keys)
    {
      if (connection.connectedRoom == roomToOpenTo)
      {
        connection.IsAccessable = true;
        connections[connection].SetActive(false);
      }
    }
  }

  public IEnumerator OpenRooms()
  {
    yield return new WaitForEndOfFrame();

    foreach (RoomConnectionInfo connection in connections.Keys)
    {
      if (connection.IsConnected)
      {
        connection.IsAccessable = true;
        connections[connection].SetActive(false);

        connection.connectedRoom.OpenRoom(this);
      }
    }
  }

  public void BuildStartWalls()
  {
    // built up walls
    var doors = new Dictionary<Vector3, GameObject>
    {
      { Vector3.back, PlaceWalls(Vector3.back, doorPrefab, wallPrefab) },
      { Vector3.forward, PlaceWalls(Vector3.forward, doorPrefab, wallPrefab) },
      { Vector3.left, PlaceWalls(Vector3.left, doorPrefab, wallPrefab) },
      { Vector3.right, PlaceWalls(Vector3.right, doorPrefab, wallPrefab) }
    };

    var buffer = new Dictionary<RoomConnectionInfo, GameObject>(connections);

    foreach (RoomConnectionInfo connection in buffer.Keys)
    {
      var direction = (connection.connectedRoom.transform.position - transform.position) / 12;

      if (doors.ContainsKey(direction))
      {
        //Debug.LogWarning("door found its connection");

        connections[connection] = doors[direction];
      }
      else
      {
        //Debug.LogError("connection didnt get object");
      }
    }

    PlaceWallFillers();
    
    // build down walls
  }

  private GameObject PlaceWalls(Vector3 direction, GameObject doorPrefab, GameObject wallPrefab)
  {
    // all rooms have the same rotation, y pos, and y scale
    #region Doors
    var doorPosition = (direction * (roomSize - 1) / 2) + (direction * 0.75F) + new Vector3(0, 1.5F, 0);

    if (direction.x < 0 || direction.z < 0)
      direction *= -1;
    var doorScale = new Vector3();
    if (direction.x == 0)
      doorScale = new Vector3(0, 3, 0) + (direction / 2F) + new Vector3(1, 0, 0);

    if (direction.z == 0)
      doorScale = new Vector3(0, 3, 0) + (direction / 2F) + new Vector3(0, 0, 1);

    GameObject door = Instantiate(doorPrefab, transform.position + doorPosition, Quaternion.identity, wallsHolders);
    door.transform.localScale = doorScale;
    #endregion

    #region Walls
    var basicWallPosition = doorPosition;

    var wallOnePos = new Vector3();
    var wallTwoPos = new Vector3();

    if (basicWallPosition.x == 0)
    {
      wallOnePos = basicWallPosition + new Vector3((int)((roomSize - 1) / 3.3F), 0, 0);
      wallTwoPos = basicWallPosition - new Vector3((int)((roomSize - 1) / 3.3F), 0, 0);
    }
    if (basicWallPosition.z == 0)
    {
      wallOnePos = basicWallPosition + new Vector3(0, 0, (int)((roomSize - 1) / 3.3F));
      wallTwoPos = basicWallPosition - new Vector3(0, 0, (int)((roomSize - 1) / 3.3F));
    }

    var wallScale = doorScale;

    if (wallScale.x == 1)
      wallScale.x = (roomSize - 1) / 2;
    if (wallScale.z == 1)
      wallScale.z = (roomSize - 1) / 2;

    Instantiate(wallPrefab, transform.position + wallOnePos, Quaternion.identity, wallsHolders).transform.localScale = wallScale;
    Instantiate(wallPrefab, transform.position + wallTwoPos, Quaternion.identity, wallsHolders).transform.localScale = wallScale;

    return door;
    #endregion
  }
  
  private void PlaceWallFillers()
  {
    var fillerScale = new Vector3(0.5F, 3, 0.5F);
    
    Instantiate(wallPrefab, transform.position + new Vector3(-5.75F, 1.5F, -5.75F), Quaternion.identity, wallsHolders).transform.localScale = fillerScale;
    Instantiate(wallPrefab, transform.position + new Vector3(-5.75F, 1.5F, 5.75F), Quaternion.identity, wallsHolders).transform.localScale = fillerScale;
    Instantiate(wallPrefab, transform.position + new Vector3(5.75F, 1.5F, -5.75F), Quaternion.identity, wallsHolders).transform.localScale = fillerScale;
    Instantiate(wallPrefab, transform.position + new Vector3(5.75F, 1.5F, 5.75F), Quaternion.identity, wallsHolders).transform.localScale = fillerScale;

  }
  
  public IEnumerator PlaceInbetweenTiles()
  {
    yield return new WaitForSeconds(1);


    foreach (RoomConnectionInfo connection in connections.Keys)
    {
      Debug.LogWarning(connection.room.indexPosition - connection.connectedRoom.indexPosition);

      if (connection.room.indexPosition - connection.connectedRoom.indexPosition == new Vector2Int(1,0) && (connection.room.roomType != RoomType.Empty && connection.connectedRoom.roomType != RoomType.Empty))
      {
        // find the connecting tile on this side
        var baseRoomTile = connection.room.transform.Find("MainRoom").Find("Row: 0").Find("0 : 5");

        // find the tile connecting tile on the other side
        var otherRoomTile = connection.connectedRoom.transform.Find("MainRoom").Find("Row: 10").Find("10 : 5");

        // make a new tile inbetween them and connect the two new tiles to the currently added tile
        var position = new Vector2(baseRoomTile.transform.position.x - 1, baseRoomTile.transform.position.z);

        var newTile = PlaceTile(position.x, position.y, greyTilePrefab, new Vector3(0,0,0), this.transform);
        newTile.GetComponent<PathingTile>().tileNode = new PathFindingNode(newTile.transform);

        newTile.GetComponent<PathingTile>().tileNode.AddConnection(baseRoomTile.GetComponent<PathingTile>().tileNode, false);
        baseRoomTile.GetComponent<PathingTile>().tileNode.AddConnection(newTile.GetComponent<PathingTile>().tileNode, false);
        gizmoConnections.Add(new GizmoConnections(newTile.GetComponent<PathingTile>().tileNode, baseRoomTile.GetComponent<PathingTile>().tileNode, false));

        newTile.GetComponent<PathingTile>().tileNode.AddConnection(otherRoomTile.GetComponent<PathingTile>().tileNode, false);
        otherRoomTile.GetComponent<PathingTile>().tileNode.AddConnection(newTile.GetComponent<PathingTile>().tileNode, false);
        gizmoConnections.Add(new GizmoConnections(newTile.GetComponent<PathingTile>().tileNode, otherRoomTile.GetComponent<PathingTile>().tileNode, false));
      }

      if (connection.room.indexPosition - connection.connectedRoom.indexPosition == new Vector2Int(0, 1) && (connection.room.roomType != RoomType.Empty && connection.connectedRoom.roomType != RoomType.Empty))
      {
        Debug.LogError("base room position: " + connection.room.indexPosition + "\n" +
          "other room position: " + connection.connectedRoom.indexPosition);

        // find the connecting tile on this side
        var baseRoomTile = connection.room.transform.Find("MainRoom").Find("Row: 5").Find("5 : 0");

        // find the tile connecting tile on the other side
        var otherRoomTile = connection.connectedRoom.transform.Find("MainRoom").Find("Row: 5").Find("5 : 10");

        // make a new tile inbetween them and connect the two new tiles to the currently added tile
        var position = new Vector2(baseRoomTile.transform.position.x, baseRoomTile.transform.position.z - 1);

        Debug.LogError("base tile position: " + baseRoomTile.position + "\n" +
          "other tile position: " + otherRoomTile.position + "\n" +
          "guess position: " + position);

        var newTile = PlaceTile(position.x, position.y, greyTilePrefab, new Vector3(0, 0, 0), this.transform);
        newTile.GetComponent<PathingTile>().tileNode = new PathFindingNode(newTile.transform);

        newTile.GetComponent<PathingTile>().tileNode.AddConnection(baseRoomTile.GetComponent<PathingTile>().tileNode, false);
        baseRoomTile.GetComponent<PathingTile>().tileNode.AddConnection(newTile.GetComponent<PathingTile>().tileNode, false);
        gizmoConnections.Add(new GizmoConnections(newTile.GetComponent<PathingTile>().tileNode, baseRoomTile.GetComponent<PathingTile>().tileNode, false));

        newTile.GetComponent<PathingTile>().tileNode.AddConnection(otherRoomTile.GetComponent<PathingTile>().tileNode, false);
        otherRoomTile.GetComponent<PathingTile>().tileNode.AddConnection(newTile.GetComponent<PathingTile>().tileNode, false);
        gizmoConnections.Add(new GizmoConnections(newTile.GetComponent<PathingTile>().tileNode, otherRoomTile.GetComponent<PathingTile>().tileNode, false));

      }
    }
  }

  public void UpdateGrizmoConnection(PathfindingNodeConnection connection)
  {
    foreach (GizmoConnections gizCon in gizmoConnections)
    {
      if (gizCon.startNode == connection.startNode && gizCon.endNode == connection.endNode ||
        gizCon.endNode == connection.startNode && gizCon.startNode == connection.endNode)
      {
        gizCon.IsConnected = connection.IsConnected;
      }
    }
  }

  private void OnDrawGizmos()
  {
    if (roomType == RoomType.Basic)
      Gizmos.color = Color.blue;

    if (roomType == RoomType.Empty)
      Gizmos.color = Color.black;

    if (roomType == RoomType.Starting)
      Gizmos.color = Color.yellow;

    Gizmos.DrawSphere(transform.position + Vector3.up, 1F);

    if (drawGizmos)
    {
      foreach (RoomConnectionInfo connection in connections.Keys)
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

    if (drawConnectionGizmos)
    {
      foreach (GizmoConnections connection in gizmoConnections)
      {
        if (connection.IsConnected)
        {
          Gizmos.color = Color.green;
          Gizmos.DrawLine(connection.positionOne, connection.positionTwo);
        }
        else
        {
          Gizmos.color = Color.red;
          Gizmos.DrawLine(connection.positionOne, connection.positionTwo);

        }
      }
    }
  }
}

[System.Serializable]
public class RoomConnectionInfo
{
  public RoomInfo room;
  public RoomInfo connectedRoom;

  public PathingTile inBetweenTile;

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

public class GizmoConnections
{
  public Vector3 positionOne;
  public Vector3 positionTwo;
  public Vector3 direction;
  public bool IsConnected;

  public PathFindingNode startNode;
  public PathFindingNode endNode;

  public GizmoConnections(PathFindingNode nodeOne, PathFindingNode nodeTwo, bool connected)
  {
    this.positionOne = nodeOne.nodeTransform.position;
    this.positionTwo = nodeTwo.nodeTransform.position;

    this.positionOne.y += .5F;
    this.positionTwo.y += .5F;

    direction = this.positionOne - this.positionTwo;

    this.IsConnected = connected;

    startNode = nodeOne;
    endNode = nodeTwo;
  }

  public void GizmoConnectionsInit(PathFindingNode nodeOne, PathFindingNode nodeTwo, bool connected)
  {
    this.positionOne = nodeOne.nodeTransform.position;
    this.positionTwo = nodeTwo.nodeTransform.position;

    this.positionOne.y = .5F;
    this.positionTwo.y = .5F;

    direction = this.positionOne - this.positionTwo;

    this.IsConnected = connected;

    startNode = nodeOne;
    endNode = nodeTwo;
  }
}
