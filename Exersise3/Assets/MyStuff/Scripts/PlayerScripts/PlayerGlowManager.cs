using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlowManager : MonoBehaviour
{
  public List<GameObject> glowableObjects = new List<GameObject>();
  public Transform camera;

  [Range(0, 10)]
  public float areaRangeRadius = 5;
  [Range(0, 90)]
  public float lookAngle = 50F;

  public float checkRange = 5;

  private void Update()
  {
    CheckForGlowableObjects();
    CheckForLooking();
    CheckToInteractWithObjest();
  }

  private void CheckForGlowableObjects()
  {
    List<GameObject> glowablesInRange = new List<GameObject>();
    // go though all the surrounding glowers and add them to glowable objects if they weren't before
    foreach (Collider col in Physics.OverlapSphere(transform.position, areaRangeRadius))
    {
      if (col.GetComponent<IGlowable>() != null)
      {
        glowablesInRange.Add(col.gameObject);

        if (!glowableObjects.Contains(col.gameObject))
        {
          glowableObjects.Add(col.gameObject);

        }
      }
    }

    List<GameObject> buffer = new List<GameObject>(glowableObjects);

    foreach (GameObject obj in buffer)
    {
      if (!glowablesInRange.Contains(obj))
      {
        if (obj.GetComponent<IGlowable>() == null)
        {
          glowableObjects.Remove(obj);

        }
        else
        {
          obj.GetComponent<IGlowable>().Glow(false);
          glowableObjects.Remove(obj);
        }
      }
    }
  }

  private void CheckForLooking()
  {
    foreach (GameObject obj in glowableObjects)
    {
      if (Vector3.Angle(camera.forward, obj.transform.position - camera.position) < lookAngle)
      {
        obj.GetComponent<IGlowable>().Glow(true);
      }
      else
      {
        obj.GetComponent<IGlowable>().Glow(false);

      }

    }
  }

  private void CheckToInteractWithObjest()
  {
    if (Input.GetKeyDown(KeyCode.E))
    {
      RaycastHit hit;
      
      if (Physics.Raycast(camera.position, camera.forward, out hit, checkRange))
      {
        if (hit.transform.GetComponent<IInteractable>() != null)
        {
          Debug.Log("Interacted with object");
          hit.transform.GetComponent<IInteractable>().Interact();

        }
      }
    }
  }
}
