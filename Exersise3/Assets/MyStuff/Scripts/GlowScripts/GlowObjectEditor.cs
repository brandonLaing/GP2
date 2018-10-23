using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GlowObject))]
public class GlowObjectEditor : Editor
{
  public override void OnInspectorGUI()
  {
    GlowObject glowObj = (GlowObject)target;
    DrawDefaultInspector();

    GUILayout.Space(10);
    if (GUILayout.Button("Start Glowing"))
    {
      glowObj.Glow(true);

    }

    GUILayout.Space(10);
    if (GUILayout.Button("Stop Glowing"))
    {
      glowObj.Glow(false);

    }
  }
}
