using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadManager : MonoBehaviour
{
  [Header("UIStuffs")]
  public Text levelNameUI;

  public void MakeNewSave()
  {
    SaveLoadSystem.MakeNewLevelSave(levelNameUI.text);
  }

  [Header("Save Info")]
  public string levelName;

  public float levelHeight;
  public float levelWidth;
  public float levelLength;

  public float glowFovAngle;
  public float glowAreaRange;

  public void SaveLevel(List<GameObject> glowingObjects, List<GameObject> obstacleObjects)
  {
    LevelData newData = new LevelData(levelNameUI.text, levelHeight, levelWidth, levelLength, glowFovAngle, glowAreaRange);

    foreach (GameObject obj in glowingObjects)
    {
      GlowingItemData glowItem = new GlowingItemData(obj.GetComponent<PropInfo>(), obj.transform);
      newData.savedGlows.Add(glowItem);
    }

    foreach (GameObject obj in obstacleObjects)
    {
      ObstacleItemData obsacleItem = new ObstacleItemData(obj.GetComponent<PropInfo>(), obj.transform);
      newData.savedObsacles.Add(obsacleItem);
    }

    SaveLoadSystem.SaveLevel(newData);
  }

  public void LoadLevel(string levelName)
  {
    LevelData loadedData = SaveLoadSystem.LoadLevel(levelName);

    this.levelName = levelName;

    SetLevelDimentions(loadedData.levelHeight, loadedData.levelWidth, loadedData.levelLength);

  }

  public void SetLevelDimentions(float height, float width, float length)
  {
    this.levelHeight = height;
    this.levelWidth = width;
    this.levelLength = length;

    throw new System.NotImplementedException();
  }

}

[System.Serializable]
public class LevelData
{
  public string LevelName { get; set; }

  public float levelHeight;
  public float levelWidth;
  public float levelLength;

  public float glowfovAngleRange;
  public float glowAreaRange;

  public List<GlowingItemData> savedGlows = new List<GlowingItemData>();
  public List<ObstacleItemData> savedObsacles = new List<ObstacleItemData>();

  public LevelData()
  {
    LevelName = string.Empty;

    levelHeight = 0;
    levelWidth = 0;
    levelLength = 0;

    glowfovAngleRange = 0;
    glowAreaRange = 0;
  }

  public LevelData(string levelName, float levelHeight, float levelWidth, float levelLength, float glowFovAngle, float glowAreaRange)
  {
    LevelName = levelName;
    this.levelHeight = levelHeight;
    this.levelWidth = levelWidth;
    this.levelLength = levelLength;
    glowfovAngleRange = glowFovAngle;
    this.glowAreaRange = glowAreaRange;
  }
}

[System.Serializable]
public class GlowingItemData
{
  public string itemName;

  public Vector3 position;
  public Vector3 rotation;
  public Vector3 scale;
 
  public bool isGlowing;
  public Vector3 glowColor;

  public GlowingItemData(PropInfo prop, Transform transform)
  {
    itemName = prop.resourceName;

    isGlowing = prop.glowing;
    glowColor = new Vector3(prop.glowColor.r, prop.glowColor.g, prop.glowColor.b);

    position = transform.position;
    rotation = transform.rotation.eulerAngles;
    scale = transform.localScale;
  }

}

[System.Serializable]
public class ObstacleItemData
{
  public string itemName;
  public Vector3 itemPosition;
  public Vector3 rotation;

  public ObstacleItemData(PropInfo prop, Transform transform)
  {
    itemName = prop.resourceName;

    itemPosition = transform.position;
    rotation = transform.eulerAngles;
  }
}