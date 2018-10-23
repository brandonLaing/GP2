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
  private void Update()
  {
    CheckForGlowableObjects();
    CheckForLooking();

  }

  private void CheckForGlowableObjects()
  {
    glowableObjects = new List<GameObject>();

    foreach (Collider col in Physics.OverlapSphere(transform.position, areaRangeRadius))
    {
      if (col.GetComponent<IGlowable>() != null)
      {
        glowableObjects.Add(col.gameObject);
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
}
