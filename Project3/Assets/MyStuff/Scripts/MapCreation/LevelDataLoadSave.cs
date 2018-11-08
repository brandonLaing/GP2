//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LevelDataLoadSave : MonoBehaviour
//{

//  public Transform movingSphere;
//  private Vector3 spherePos;
//  void Update()
//  {

//  }

//  private void OnGUI()
//  {
//    if (GUILayout.Button("Load Level"))
//    {
//      movingSphere.position = spherePos;
//    }
//    if (GUILayout.Button("Save Level"))
//    {
//      spherePos = movingSphere.position;
//    }
//  }
//}

//[System.Serializable]
//public class LevelData
//{
//  public List<MovingSphereData> movingSpheres = new List<MovingSphereData>();


//  public void SaveToFile(string fileName)
//  {
//    string jsonLevel = JsonUtility.ToJson(this);
//    System.IO.File.WriteAllText(fileName, jsonLevel);

//  }

//  public void LoadFromFile(string fileName)
//  {
//    LevelData loaded = JsonUtility.FromJson<LevelData>(System.IO.File.ReadAllText(fileName));
//  }
//}

//[System.Serializable]
//public class MovingSphereData
//{
//  public Vector3 position;
//  public float moveSpeed;
//}



