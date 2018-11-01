using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlowManager : MonoBehaviour
{
  public List<GameObject> glowableObjects = new List<GameObject>();
  public Transform cameraTranform;

  [Range(1, 20)]
  public float areaRangeRadius = 5;
  [Range(10, 90)]
  public float lookAngle = 50F;
  [Range(1,10)]
  public float checkRange = 5;


  private void Update()
  {
    CheckForGlowableObjects();
    CheckForLooking();
    CheckToInteractWithObjest();
  }

  // check if there are any glowing objects around us
  private void CheckForGlowableObjects()
  {
    // set up a container for any glowables in our range
    List<GameObject> glowablesInRange = new List<GameObject>();

    // go though all the surrounding glowers and add them to glowables in range and if they arent already in the list of glowable object add them to that too
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

    // now make a buffer for glowable objects
    List<GameObject> buffer = new List<GameObject>(glowableObjects);

    // go though that buffer and check if there are any objects in our glowable objects that arent in range anymore and if they arent in range turn off the glow and remove them from our glowable object
    // but if they dont have a glow just remove them.
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

  // finds if the player is looking at a object
  private void CheckForLooking()
  {
    // for every potential glowable in our area
    foreach (GameObject obj in glowableObjects)
    {
      // check if we are within the angle
      if (Vector3.Angle(cameraTranform.forward, obj.transform.position - cameraTranform.position) < lookAngle)
      {
        // if so glow that object
        obj.GetComponent<IGlowable>().Glow(true);
      }
      else
      {
        // else stop glow
        obj.GetComponent<IGlowable>().Glow(false);
      }
    }
  }

  // checks if we interacted with an object
  private void CheckToInteractWithObjest()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      RaycastHit hit;
      
      if (Physics.Raycast(cameraTranform.position, cameraTranform.forward, out hit, checkRange))
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
