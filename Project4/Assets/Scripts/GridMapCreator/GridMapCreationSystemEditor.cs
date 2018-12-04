using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridMapCreationSystem))]
public class GridMapCreationSystemEditor : Editor
{
  public override void OnInspectorGUI()
  {
    GridMapCreationSystem targetScript = (GridMapCreationSystem)target;

    DrawDefaultInspector();

    GUILayout.Space(10);

    if (GUILayout.Button("Rebuild Everything"))
    {
      targetScript = new GridMapCreationSystem();
    }
  }
}
