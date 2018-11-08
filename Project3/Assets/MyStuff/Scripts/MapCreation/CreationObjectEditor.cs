using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreationObjectEditor : MonoBehaviour
{
  public GameObject target;
  public Camera myCam;
  public CreationObjectPositioner positioner;

  public Text nameText;

  public InputField[] positions;
  public InputField[] rotations;
  public InputField[] scales;

  public Button glowButton;

  public InputField[] colors;

  public Image colorsBackground;

  private void Update()
  {
    CheckForNewTarget();
    CheckToFocus();

  }

  private void CheckToFocus()
  {
    if (Input.GetKeyDown(KeyCode.F) && target != null)
      myCam.GetComponent<CreationCameraScript>().Focus(target);
  }

  private void CheckForNewTarget()
  {
    if (Input.GetMouseButtonDown(0))
    {
      RaycastHit hit;
      Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out hit))
      {
        Transform objectHit = hit.transform;

        if (objectHit.tag == "Props")
        {
          target = objectHit.gameObject;
          positioner.SetNewTarget(objectHit);
          UpdateUiTransformInfo();
          SetButtonGlow();
          SetNewTargetsGlow();
        }
      }
    }
  }

  #region Object Glow
  public void UpdateObjectGlow()
  {
    float newX, newY, newZ;

    float.TryParse(colors[0].text, out newX);
    float.TryParse(colors[1].text, out newY);
    float.TryParse(colors[2].text, out newZ);

    if (newX < 0)
    {
      newX = 0;
    }
    else if (newX > 255)
    {
      newX = 255;
    }
    colors[0].text = newX.ToString();

    if (newY < 0)
    {
      newY = 0;
    }
    else if (newY > 255)
    {
      newY = 255;
    }
    colors[1].text = newY.ToString();

    if (newZ < 0)
    {
      newZ = 0;
    }
    else if (newZ > 255)
    {
      newZ = 255;
    }
    colors[2].text = newZ.ToString();

    colorsBackground.color = new Color(newX / 255, newY / 255, newZ / 255);

    if (target != null)
    {
      target.GetComponent<PropInfo>().glowColor = colorsBackground.color;
    }

  }

  public void SetNewTargetsGlow()
  {
    colors[0].text = (target.GetComponent<PropInfo>().glowColor.r * 255).ToString();
    colors[1].text = (target.GetComponent<PropInfo>().glowColor.g * 255).ToString();
    colors[2].text = (target.GetComponent<PropInfo>().glowColor.b * 255).ToString();

  }
  #endregion

  #region Glow Button
  public void UpdateTargetIsGlowing()
  {
    if (target != null)
    {
      PropInfo prop = target.GetComponent<PropInfo>();

      prop.glowing = !prop.glowing;

      if (prop.glowing)
      {
        glowButton.GetComponent<Image>().color = Color.green;

      }
      else
      {
        glowButton.GetComponent<Image>().color = Color.red;

      }
      return;
    }

    glowButton.GetComponent<Image>().color = Color.white;
  }

  public void SetButtonGlow()
  {
    PropInfo prop = target.GetComponent<PropInfo>();
    if (prop.glowing)
    {
      glowButton.image.color = Color.green;

    }
    else
    {
      glowButton.image.color = Color.red;

    }
  }
  #endregion

  #region Transform UI
  public void UpdateUiTransformInfo()
  {
    if (target.GetComponent<PropInfo>())
    {
      nameText.text = target.GetComponent<PropInfo>().resourceName;
    }

    positions[0].text = target.transform.position.x.ToString();
    positions[1].text = target.transform.position.y.ToString();
    positions[2].text = target.transform.position.z.ToString();

    rotations[0].text = target.transform.rotation.x.ToString();
    rotations[1].text = target.transform.rotation.y.ToString();
    rotations[2].text = target.transform.rotation.z.ToString();

    scales[0].text = target.transform.localScale.x.ToString();
    scales[1].text = target.transform.localScale.y.ToString();
    scales[2].text = target.transform.localScale.z.ToString();

  }

  public void UpdatePositionFromUI()
  {
    float newX, newY, newZ;

    float.TryParse(positions[0].text, out newX);
    float.TryParse(positions[1].text, out newY);
    float.TryParse(positions[2].text, out newZ);

    Vector3 uiPosition = new Vector3(newX, newY, newZ);

    positioner.MoveTarget(uiPosition);

  }

  public void UpdateRotationFromUI()
  {
    float newX, newY, newZ;

    float.TryParse(rotations[0].text, out newX);
    float.TryParse(rotations[1].text, out newY);
    float.TryParse(rotations[2].text, out newZ);

    Vector3 uiRotations = new Vector3(newX, newY, newZ);
    target.transform.eulerAngles = uiRotations;
  }

  public void UpdateScaleFromUI()
  {
    float newX, newY, newZ;

    float.TryParse(scales[0].text, out newX);
    float.TryParse(scales[1].text, out newY);
    float.TryParse(scales[2].text, out newZ);

    Vector3 uiScales = new Vector3(newX, newY, newZ);
    target.transform.localScale = uiScales;
  }
  #endregion
}
