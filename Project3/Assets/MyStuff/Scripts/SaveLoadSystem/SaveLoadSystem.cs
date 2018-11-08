using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadSystem
{
  public static string SavesDirectory { get { return Directory.GetCurrentDirectory() + "/SavedLevels/"; } }

  public static void MakeDirectory()
  {
    if (!Directory.Exists(SavesDirectory))
    {
      Directory.CreateDirectory(SavesDirectory);
    }
  }

  public static bool MakeNewLevelSave(string saveName)
  {
    if (MakeNewLevelSave(saveName, new LevelData()))
    {
      return true;
    }

    return false;
  }

  public static bool MakeNewLevelSave(string saveName, LevelData saveData)
  {
    // check if saves directory exist and if not make directory
    if (!Directory.Exists(SavesDirectory))
    {
      Directory.CreateDirectory(SavesDirectory);
    }

    // check if there isn't a file there with the same name already start making the empty file
    if (!File.Exists(SavesDirectory + saveName + "map.sav") && saveData != null && saveName != null && saveName != string.Empty)
    {
      BinaryFormatter bf = new BinaryFormatter();
      FileStream stream = new FileStream(SavesDirectory + saveName + "_Save.map", FileMode.Create);

      bf.Serialize(stream, saveData);

      stream.Close();

      return true;
    }

    return false;
  }

  public static void SaveLevel(LevelData saveData)
  {
    if (saveData != null)
    {
      if (File.Exists(SavesDirectory + saveData.LevelName + "_map.sav"))
      {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(SavesDirectory + saveData.LevelName + "_map.sav", FileMode.Create);

        bf.Serialize(stream, saveData);

        stream.Close();
      }
      else
      {
        MakeNewLevelSave(saveData.LevelName, saveData);
      }
    }
  }

  public static LevelData LoadLevel(string saveName)
  {
    if (File.Exists(SavesDirectory + saveName + "_map.sav"))
    {
      BinaryFormatter bf = new BinaryFormatter();
      FileStream stream = new FileStream(SavesDirectory + saveName + "_map.sav", FileMode.Open);

      LevelData saveData = bf.Deserialize(stream) as LevelData;

      stream.Close();

      return saveData;
    }

    return null;
  }
}
