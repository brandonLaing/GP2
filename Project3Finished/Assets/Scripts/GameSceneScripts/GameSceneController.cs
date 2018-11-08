using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class GameSceneController : MonoBehaviour
{
  public LevelData currentData;

  public GameObject levelChooser;
  public GameObject levelHolder;
  public GameObject levelChooserPrefab;

  public GameObject menuBackground;

  public GameObject map;
  public List<GameObject> allLevelChoosers = new List<GameObject>();

  public GameObject player;

  public List<GameObject> glowableObjects = new List<GameObject>();

  [Header("Geometry")]
  public GameObject floor;
  public GameObject roof, backWall, frontWall, leftWall, rightWall, levelLight;

  private void Start()
  {
    OpenLevelChooser();
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      menuBackground.SetActive(!menuBackground.activeSelf);
    }
  }

  public void OpenLevelChooser()
  {
    player.GetComponent<PlayerMovementControls>().enabled = false;
    player.GetComponentInChildren<PlayerCameraControls>().enabled = false;

    levelChooser.SetActive(true);

    foreach (GameObject obj in allLevelChoosers)
    {
      Destroy(obj);
    }

    allLevelChoosers = new List<GameObject>();

    foreach (string levelName in SaveLoadSystem.GetAllLevels())
    {
      GameObject newButton = Instantiate(levelChooserPrefab, levelHolder.transform);
      newButton.GetComponentInChildren<UnityEngine.UI.Text>().text = levelName;
      newButton.GetComponent<GameLevelChooserScript>().sceneController = this;
      allLevelChoosers.Add(newButton);
    }
  }

  public void LoadLevel(string levelName)
  {
    LevelData workingSave = SaveLoadSystem.LoadLevel(levelName);

    currentData = workingSave;

    SetUpLevelGeometry(currentData);
    SetUpGlowSetting(currentData);
    SetUpObjects(currentData);


    player.GetComponent<PlayerMovementControls>().enabled = true;
    player.GetComponentInChildren<PlayerCameraControls>().enabled = true;
    levelChooser.SetActive(false);

    StartedNewLevel();
  }

  private void SetUpObjects(LevelData currentData)
  {
    foreach (GameObject obj in allObjects)
    {
      Destroy(obj);
    }

    allObjects = new List<GameObject>();
    glowableObjects = new List<GameObject>();

    int i = 0;
    foreach (GlowingItemData data in currentData.savedGlows)
    {
      GameObject newObj = Instantiate(Resources.Load(data.resourceName, typeof(GameObject)), Vector3.zero, Quaternion.identity, map.transform) as GameObject;
      allObjects.Add(newObj);

      newObj.name += i;

      newObj.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
      newObj.transform.eulerAngles = new Vector3(data.rotation[0], data.rotation[1], data.rotation[2]);
      newObj.transform.localScale = new Vector3(data.scale[0], data.scale[1], data.scale[2]);

      LevelObjectInfo objectInfo = newObj.GetComponent<LevelObjectInfo>();
      objectInfo.isGlowing = data.isGlowing;
      objectInfo.SetGlow(new Vector3(data.glowColor[0], data.glowColor[1], data.glowColor[2]));

      if (objectInfo.isGlowing)
      {
        glowableObjects.Add(newObj);
        newObj.AddComponent<GlowObject>().glowColor= objectInfo.glowColor;
      }

      i++;
    }
  }

  public void SetUpLevelGeometry(LevelData saveData)
  {
    // set positions
    roof.transform.position = new Vector3(0, saveData.levelHeight, 0);

    frontWall.transform.position = new Vector3(0, saveData.levelHeight / 2, -saveData.levelLength / 2);
    backWall.transform.position = new Vector3(0, saveData.levelHeight / 2, saveData.levelLength / 2);

    leftWall.transform.position = new Vector3(-saveData.levelWidth / 2, saveData.levelHeight / 2, 0);
    rightWall.transform.position = new Vector3(saveData.levelWidth / 2, saveData.levelHeight / 2, 0);

    levelLight.transform.position = new Vector3(0, saveData.levelHeight, 0);

    // set scales
    floor.transform.localScale = new Vector3(saveData.levelWidth, saveData.levelLength, 1);
    roof.transform.localScale = new Vector3(saveData.levelWidth, saveData.levelLength, 1);

    frontWall.transform.localScale = new Vector3(saveData.levelWidth, saveData.levelHeight, 1);
    backWall.transform.localScale = new Vector3(saveData.levelWidth, saveData.levelHeight, 1);

    leftWall.transform.localScale = new Vector3(saveData.levelLength, saveData.levelHeight, 1);
    rightWall.transform.localScale = new Vector3(saveData.levelLength, saveData.levelHeight, 1);
  }

  public void SetUpGlowSetting(LevelData saveData)
  {
    player.GetComponent<PlayerGlowManager>()
      .areaRangeRadius = saveData.glowAreaRange;
    player.GetComponent<PlayerGlowManager>()
      .lookAngle = saveData.glowFovLookAngle;
  }

  public DateTime startTime;

  public void StartedNewLevel()
  {
    startTime = DateTime.Now;
  }

  public Dictionary<string, object> foundObjects = new Dictionary<string, object>();
  public List<GameObject> allObjects = new List<GameObject>();
  public void ObjectFound(GameObject gameObject, TimeSpan timeToFind)
  {
    foundObjects.Add(gameObject.name, timeToFind.Seconds);

    glowableObjects.Remove(gameObject);

    if (glowableObjects.Count == 0)
    {
      Analytics.CustomEvent("player_finished_level", foundObjects);
      foundObjects = new Dictionary<string, object>();

      OpenLevelChooser();
    }
  }

  public void BackToMainMenu()
  {
    SceneManager.LoadScene("MainMenu");
  }
}
