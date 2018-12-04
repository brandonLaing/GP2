using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathingTile))]
public class PathingTileEditor : Editor
{
  public override void OnInspectorGUI()
  {
    var targetScript = target as PathingTile;

    DrawDefaultInspector();

    GUILayout.Space(10);
    if (GUILayout.Button("Open Connections"))
    {
      targetScript.tileNode.Unblock();
    }

    GUILayout.Space(10);
    if (GUILayout.Button("Close Connections"))
    {
      targetScript.tileNode.Block();
    }
  }
}
