using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjectInfo : MonoBehaviour
{
  public string resourceName;

  public bool isGlowing;
  public Color glowColor;

  public void SetGlow(Color glowColor)
  {
    this.glowColor = glowColor;
  }

  public void SetGlow(Vector3 glowColor)
  {
    this.glowColor = new Color(glowColor.x, glowColor.y, glowColor.z);
  }

  public Vector3 GetGlowAsVector()
  {
    return new Vector3(glowColor.r, glowColor.g, glowColor.b);
  }

  public void CheckObjectConstraints()
  {
    //throw new System.NotImplementedException();
  }
}
