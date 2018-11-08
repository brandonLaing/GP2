using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadSystem
{
  private static string SavesDirectory { get { return Directory.GetCurrentDirectory() + "/SavedLevels/"; } }
  private static string SaveExtention { get { return ".map"; } }


  // checks if the saves directory exist and if it doesnt it makes that folder
  public static void MakeSureSavesExist()
  {
    if (!Directory.Exists(SavesDirectory))
    {
      Directory.CreateDirectory(SavesDirectory);
    }
  }

  // makes a new save data with a premade level data
  public static void MakeNewLevelSave(string saveName, LevelData saveData)
  {
    MakeSureSavesExist();

    saveName = saveName.Trim();


    if (!File.Exists(SavesDirectory + saveName + ".map") && saveData != null && saveName != null && saveName != string.Empty)
    {
      BinaryFormatter bf = new BinaryFormatter();
      FileStream fs = new FileStream(SavesDirectory + saveName + SaveExtention, FileMode.Create);

      bf.Serialize(fs, saveData);
      fs.Close();
    }
  }

  // makes a new save data with empty level data
  public static void MakeNewLevelSave(string saveName)
  {
    MakeNewLevelSave(saveName, new LevelData());

  }

  // takes a level data and saves it
  public static void SaveLevel(LevelData saveData)
  {
    MakeSureSavesExist();

    if (saveData != null)
    {
      if (File.Exists(SavesDirectory + saveData.saveName + SaveExtention))
      {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(SavesDirectory + saveData.saveName + SaveExtention, FileMode.Create);

        bf.Serialize(fs, saveData);
        fs.Close();
      }
      else
      {
        MakeNewLevelSave(saveData.saveName, saveData);
      }
    }
  }

  public static string[] GetAllLevels()
  {
    MakeSureSavesExist();

    string[] rawArray = Directory.GetFiles(SavesDirectory);

    string[] refindedArray = new string[rawArray.Length];

    for (int i = 0; i < rawArray.Length; i++)
    {
      refindedArray[i] = rawArray[i]
                                    .Replace(SavesDirectory, "")
                                    .Replace(SaveExtention, "");
    }

    return refindedArray;
  }

  // finds a level data and loads it from folder
  public static LevelData LoadLevel(string saveName)
  {
    MakeSureSavesExist();

    if (File.Exists(SavesDirectory + saveName + SaveExtention))
    {
      BinaryFormatter bf = new BinaryFormatter();
      FileStream fs = new FileStream(SavesDirectory + saveName + SaveExtention, FileMode.Open);

      LevelData saveData = bf.Deserialize(fs) as LevelData;
      fs.Close();
      return saveData;
    }

    return null;
  }
}

[Serializable]
public class LevelData
{
  public LevelData()
  {
    saveName = "defaultName";

    levelLength = 10; levelWidth = 10; levelHeight = 5;

    glowFovLookAngle = 45; glowAreaRange = 2;
  }

  public LevelData(string saveName, float levelLength, float levelWidth, float levelHeight, float glowFovLookAngle, float glowAreaRange)
  {
    this.saveName = saveName;

    this.levelLength = levelLength; this.levelWidth = levelWidth; this.levelHeight = levelHeight;

    this.glowFovLookAngle = glowFovLookAngle; this.glowAreaRange = glowAreaRange;
  }

  public string saveName;

  public float levelLength, levelWidth, levelHeight;

  public float glowFovLookAngle, glowAreaRange;

  public List<GlowingItemData> savedGlows = new List<GlowingItemData>();
  public List<ObstacleItemData> savedObstacles = new List<ObstacleItemData>();

  public void AddGlowingItem(GameObject glowingItemObj)
  {
    savedGlows.Add(new GlowingItemData(glowingItemObj));
  }

  public void AddObstacleItem(GameObject obstacleObj)
  {
    savedObstacles.Add(new ObstacleItemData(obstacleObj));
  }
}

[Serializable]
public class GlowingItemData
{
  public string resourceName;

  public float[] position = new float[3], rotation = new float[3], scale = new float[3];

  public bool isGlowing;
  public float[] glowColor = new float[3];

  public GlowingItemData(GameObject itemObj)
  {
    if (itemObj.GetComponent<LevelObjectInfo>())
    {
      LevelObjectInfo objectInfo = itemObj.GetComponent<LevelObjectInfo>();

      resourceName = objectInfo.resourceName;

      this.position = new[] { itemObj.transform.position.x, itemObj.transform.position.y, itemObj.transform.position.z };
      this.rotation = new[] { itemObj.transform.eulerAngles.x, itemObj.transform.eulerAngles.y, itemObj.transform.eulerAngles.z };
      this.scale = new[] { itemObj.transform.localScale.x, itemObj.transform.localScale.y, itemObj.transform.localScale.z };

      isGlowing = objectInfo.isGlowing;
      Vector3 temp = objectInfo.GetGlowAsVector();
      glowColor = new[] { temp.x, temp.y, temp.z };
    }
  }
}

[Serializable]
public class ObstacleItemData
{
  public string resourceName;

  public float[] position = new float[3], rotation = new float[3], scale = new float[3];

  public ObstacleItemData(GameObject itemObj)
  {
    if (itemObj.GetComponent<LevelObjectInfo>())
    {
      LevelObjectInfo objectInfo = itemObj.GetComponent<LevelObjectInfo>();

      resourceName = objectInfo.resourceName;

      this.position = new[] { itemObj.transform.position.x, itemObj.transform.position.y, itemObj.transform.position.z };
      this.rotation = new[] { itemObj.transform.eulerAngles.x, itemObj.transform.eulerAngles.y, itemObj.transform.eulerAngles.z };
      this.scale = new[] { itemObj.transform.localScale.x, itemObj.transform.localScale.y, itemObj.transform.localScale.z };
    }
  }
}