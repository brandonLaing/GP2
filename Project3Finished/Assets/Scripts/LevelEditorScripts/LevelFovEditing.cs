using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFovEditing : MonoBehaviour
{
  public float glowFovLookAngle, glowAreaRange;

  public InputField[] fields;

  private void Start()
  {
    SetFovLook(fields[0]);
    SetFovRange(fields[1]);
  }

  public void SetFovLook(InputField field)
  {
    int newAngle;
    int.TryParse(field.text, out newAngle);

    if (newAngle < 10)
    {
      newAngle = 10;
    }
    if (newAngle > 360)
    {
      newAngle = 360;
    }

    field.text = newAngle.ToString();

    glowFovLookAngle = newAngle;
  }

  public void SetFovRange(InputField field)
  {
    float newRange;
    float.TryParse(field.text, out newRange);

    if (newRange < 1)
    {
      newRange = 1;
    }

    if (newRange > 20)
    {
      newRange = 20;
    }

    field.text = newRange.ToString();

    glowAreaRange = newRange;
  }

  public void SetGlowSettings(LevelData saveData)
  {
    glowFovLookAngle = saveData.glowFovLookAngle;
    glowAreaRange = saveData.glowAreaRange;

    fields[0].text = glowFovLookAngle.ToString();
    fields[1].text = glowAreaRange.ToString();

    SetFovLook(fields[0]);
    SetFovRange(fields[1]);
  }
}
