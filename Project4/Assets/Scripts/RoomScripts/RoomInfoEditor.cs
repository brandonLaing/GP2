using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomInfo))]
public class RoomInfoEditor : Editor
{
  public override void OnInspectorGUI()
  {
    RoomInfo targetScript = (RoomInfo)target;

    DrawDefaultInspector();

    GUILayout.Space(10);
    if (GUILayout.Button("Open Rooms connections"))
    {
      targetScript.StartCoroutine(targetScript.OpenRooms());
    }

    GUILayout.Space(10);
    foreach (RoomConnectionInfo connection in targetScript.connections.Keys)
    {
      if (connection.IsConnected)
      {
        if (GUILayout.Button("Open To Room " + connection.connectedRoom.name))
        {
          targetScript.OpenRoom(connection.connectedRoom);
          connection.connectedRoom.OpenRoom(targetScript);
        }
      }
    }
  }
}
