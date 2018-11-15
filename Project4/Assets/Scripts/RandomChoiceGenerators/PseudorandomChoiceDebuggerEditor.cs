using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PseudorandomChoiceDebugger))]
public class PseudorandomChoiceDebuggerEditor : Editor
{
  public override void OnInspectorGUI()
  {
    PseudorandomChoiceDebugger debuggerTarget = (PseudorandomChoiceDebugger)target;

    DrawDefaultInspector();

    GUILayout.Space(10);
    if (GUILayout.Button("Run 1,000,000 times"))
    {
      for (int i = 0; i < 10000000; i++)
      {
        debuggerTarget.RunSystem();
      }
    }
  }
}
