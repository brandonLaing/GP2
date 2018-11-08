using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LevelObjectsEditing : MonoBehaviour
{
  public LevelGlowEditing glowEditor;
  public LevelObjectPlacementEditing objectPlacer;
  public Text objectUiName;

  public List<GameObject> glowableObjects = new List<GameObject>();
  public List<GameObject> obstacleObjects = new List<GameObject>();

  private Transform objectHolder;

  public LevelObjectInfo targetObject;
  public LayerMask objectLayer;

  private void Start()
  {
    objectHolder = new GameObject().transform;
    objectHolder.gameObject.name = "ObjectHolder";
    if (glowEditor == null)
    {
      glowEditor = GetComponentInChildren<LevelGlowEditing>();
    }
    if (objectPlacer == null)
    {
      objectPlacer = GetComponentInChildren<LevelObjectPlacementEditing>();
    }
  }

  private void Update()
  {
    CheckForNewTarget();
    CheckForDeleteObject();
    if (Input.GetKeyDown(KeyCode.F) && targetObject != null)
    {
      Camera.main.transform.position = targetObject.transform.position + new Vector3(-2, 2, 0);
      Camera.main.transform.LookAt(targetObject.transform);
    }
  }

  private void CheckForDeleteObject()
  {
    if (targetObject != null && Input.GetKeyDown(KeyCode.Delete))
    {
      RemoveGlowable(targetObject.gameObject);
      targetObject = null;
      NewObjectTargeted(null); 
    }
  }

  private void CheckForNewTarget()
  {
    if (Input.GetMouseButtonDown(0))
    {
      RaycastHit hit;
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out hit, 100, objectLayer))
      {
        NewObjectTargeted(hit.transform.gameObject);
      }
    }
  }

  public void NewObjectTargeted(GameObject newTarget)
  {
    if (newTarget != null)
    {
      targetObject = newTarget.GetComponent<LevelObjectInfo>(); ;

      if (targetObject.tag == "Props")
      {
        glowEditor.gameObject.SetActive(true);
        glowEditor.SetNewTargetsColor();
        glowEditor.SetNewTargetsGlow();

        objectPlacer.gameObject.SetActive(true);
        objectPlacer.SetAllInputs();

        objectUiName.gameObject.SetActive(true);
        objectUiName.text = targetObject.resourceName.Replace("prop_", "");
      }
    }
    else
    {
      glowEditor.gameObject.SetActive(false);
      objectPlacer.gameObject.SetActive(false);
      objectUiName.gameObject.SetActive(false);
    }
  }


  #region Object Spawning
  public void SpawnGlowable(GameObject gameObj)
  {
    GameObject newObj = Instantiate(gameObj, Vector3.zero, Quaternion.identity, objectHolder);
    glowableObjects.Add(newObj);
    NewObjectTargeted(newObj);
  }

  public void SpawnGlowable(GlowingItemData itemData)
  {
    GameObject newObj = Instantiate(Resources.Load(itemData.resourceName, typeof(GameObject)), Vector3.zero, Quaternion.identity, objectHolder) as GameObject;
    glowableObjects.Add(newObj);

    newObj.transform.position = new Vector3(itemData.position[0], itemData.position[1], itemData.position[2]);
    newObj.transform.eulerAngles = new Vector3(itemData.rotation[0], itemData.rotation[1], itemData.rotation[2]);
    newObj.transform.localScale = new Vector3(itemData.scale[0], itemData.scale[1], itemData.scale[2]);
    
    LevelObjectInfo objectInfo = newObj.GetComponent<LevelObjectInfo>();
    objectInfo.isGlowing = itemData.isGlowing;
    objectInfo.SetGlow(new Vector3(itemData.glowColor[0], itemData.glowColor[1], itemData.glowColor[2]));
  }

  public void RemoveGlowable(GameObject gameObj)
  {
    glowableObjects.Remove(gameObj);
    Destroy(gameObj);

  }

  public void SpawnObstacle(GameObject gameObj)
  {
    GameObject newObj = Instantiate(gameObj, Vector3.zero, Quaternion.identity, objectHolder);
    glowableObjects.Add(newObj);
    NewObjectTargeted(newObj);
  }

  public void SpawnObstacle(ObstacleItemData itemData)
  {
    GameObject newObj = Instantiate(Resources.Load(itemData.resourceName, typeof(GameObject)), Vector3.zero, Quaternion.identity, objectHolder) as GameObject;
    glowableObjects.Add(newObj);

    newObj.transform.position = new Vector3(itemData.position[0], itemData.position[1], itemData.position[2]);
    newObj.transform.eulerAngles = new Vector3(itemData.rotation[0], itemData.rotation[1], itemData.rotation[2]);
    newObj.transform.localScale = new Vector3(itemData.scale[0], itemData.scale[1], itemData.scale[2]);
  }

  public void RemoveObstacle(GameObject gameObj)
  {
    glowableObjects.Remove(gameObj);
    Destroy(gameObj);
  }

  public void Clear()
  {
    foreach (GameObject obj in glowableObjects)
    {
      Destroy(obj);
    }
    glowableObjects = new List<GameObject>();

    foreach (GameObject obj in obstacleObjects)
    {
      Destroy(obj);
    }
    obstacleObjects = new List<GameObject>();
  }
  #endregion
}
