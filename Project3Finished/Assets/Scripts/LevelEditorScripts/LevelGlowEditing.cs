using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LevelGlowEditing : MonoBehaviour
{
  public LevelObjectsEditing objectEditor;

  public InputField[] rgbFields;

  public Image backgroundColor;
  public Button isGlowingButton;

  private float _r;
  private float R
  {
    get
    {
      return _r;
    }
    set
    {
      if (value < 0)
      {
        _r = 0;
        return;
      }

      if (value > 255)
      {
        _r = 255;
        return;
      }

      _r = value;
    }
  }

  private float _g;
  private float G
  {
    get
    {
      return _g;
    }
    set
    {
      if (value < 0)
      {
        _g = 0;
        return;
      }

      if (value > 255)
      {
        _g = 255;
        return;
      }

      _g = value;
    }
  }

  private float _b;
  private float B
  {
    get
    {
      return _b;
    }
    set
    {
      if (value < 0)
      {
        _b = 0;
        return;
      }

      if (value > 255)
      {
        _b = 255;
        return;
      }

      _b = value;
    }
  }


  public void UpdateGlowInputs()
  {
    float newX, newY, newZ;

    // get new color from text boxes
    float.TryParse(rgbFields[0].text, out newX);
    float.TryParse(rgbFields[1].text, out newY);
    float.TryParse(rgbFields[2].text, out newZ);

    // set those into properties
    R = newX;
    G = newY;
    B = newZ;

    // set the properties back into the text boxes
    rgbFields[0].text = R.ToString();
    rgbFields[1].text = G.ToString();
    rgbFields[2].text = B.ToString();

    Color adjustedColor = new Color(R / 255, G / 255, B / 255);

    // set background color
    backgroundColor.color = adjustedColor;

    // send that color to object
    objectEditor.targetObject.SetGlow(adjustedColor);
  }

  public void SetNewTargetsColor()
  {
    rgbFields[0].text = (objectEditor.targetObject.glowColor.r * 255).ToString();
    rgbFields[1].text = (objectEditor.targetObject.glowColor.g * 255).ToString();
    rgbFields[2].text = (objectEditor.targetObject.glowColor.b * 255).ToString();

    UpdateGlowInputs();
  }

  public void ChangeGlow()
  {
    objectEditor.targetObject.isGlowing = !objectEditor.targetObject.isGlowing;
    SetNewTargetsGlow();
  }

  public void SetNewTargetsGlow()
  {
    if (objectEditor.targetObject.isGlowing)
    {
      isGlowingButton.targetGraphic.color = Color.green;
    }
    else
    {
      isGlowingButton.targetGraphic.color = Color.red;
    }
  }
}
