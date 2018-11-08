using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadLevel : MonoBehaviour
{
  public LevelObjectsEditing objectEditor;
  public LevelFovEditing fovEditor;
  public LevelGeometryEditing geometryEditor;
  public LevelChooser levelChooser;

  public LevelData currentLevelData;

  public void SaveLevel(string levelName)
  {
    // get level dimentions
    float levelLength = geometryEditor.levelLength,
      levelWidth = geometryEditor.levelWidth,
      levelHeight = geometryEditor.levelHeight;

    // get glow data
    float glowFovLookAngle = fovEditor.glowFovLookAngle, glowAreaRange = fovEditor.glowAreaRange;

    LevelData workingSave = new LevelData(levelName, levelLength, levelWidth, levelHeight, glowFovLookAngle, glowAreaRange);

    // get glow objects
    foreach (GameObject obj in objectEditor.glowableObjects)
    {
      workingSave.AddGlowingItem(obj);
    }

    // get obstical objects
    foreach (GameObject obj in objectEditor.obstacleObjects)
    {
      workingSave.AddObstacleItem(obj);
    }

    SaveLoadSystem.SaveLevel(workingSave);
    currentLevelData = workingSave;
  }

  public void LoadLevel(string levelName)
  {
    LevelData workingSave = SaveLoadSystem.LoadLevel(levelName);

    fovEditor.SetGlowSettings(workingSave);
    geometryEditor.SetLevelGeometry(workingSave);

    objectEditor.Clear();


    foreach (GlowingItemData itemData in workingSave.savedGlows)
    {
      objectEditor.SpawnGlowable(itemData);
    }

    foreach (ObstacleItemData itemData in workingSave.savedObstacles)
    {
      objectEditor.SpawnObstacle(itemData);
    }

    levelChooser.LevelLoaded(levelName);
    objectEditor.NewObjectTargeted(null);
    currentLevelData = workingSave;
  }
}
