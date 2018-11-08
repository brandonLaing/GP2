using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGeometryEditing : MonoBehaviour {

  public float levelLength, levelWidth, levelHeight;
  public InputField[] heightLengthWidth;

  public GameObject floor, roof, frontWall, backWall, leftWall, rightWall, levelLight;

  private void Start()
  {
    SetLevelLength(heightLengthWidth[0]);
    SetLevelWidth(heightLengthWidth[1]);
    SetLevelHeight(heightLengthWidth[2]);
  }

  public void SetLevelLength(InputField field)
  {
    float newLength;
    float.TryParse(field.text, out newLength);

    if (newLength < 5)
    {
      newLength = 5;
    }

    if (newLength > 20)
    {
      newLength = 20;
    }

    levelLength = newLength;
    field.text = levelLength.ToString();
    UpdateLevelGeometry();
  }

  public void SetLevelWidth(InputField field)
  {
    float newWidth;
    float.TryParse(field.text, out newWidth);

    if (newWidth < 5)
    {
      newWidth = 5;
    }

    if (newWidth > 20)
    {
      newWidth = 20;
    }

    levelWidth = newWidth;
    field.text = levelWidth.ToString();
    UpdateLevelGeometry();
  }

  public void SetLevelHeight(InputField field)
  {
    float newHeight;
    float.TryParse(field.text, out newHeight);

    if (newHeight < 3)
    {
      newHeight = 3;
    }

    if (newHeight > 10)
    {
      newHeight = 10;
    }

    levelHeight = newHeight;
    field.text = levelHeight.ToString();
    UpdateLevelGeometry();
  }

  public void SetLevelGeometry(LevelData saveData)
  {
    levelLength = saveData.levelLength;
    levelWidth = saveData.levelWidth;
    levelHeight = saveData.levelHeight;

    heightLengthWidth[0].text = levelLength.ToString();
    heightLengthWidth[1].text = levelWidth.ToString();
    heightLengthWidth[2].text = levelHeight.ToString();

    SetLevelLength(heightLengthWidth[0]);
    SetLevelWidth(heightLengthWidth[1]);
    SetLevelHeight(heightLengthWidth[2]);

  }

  public void UpdateLevelGeometry()
  {
    // update the positions
    roof.transform.position = new Vector3(0, levelHeight, 0);

    frontWall.transform.position = new Vector3(0, levelHeight / 2, -levelLength / 2);
    backWall.transform.position = new Vector3(0, levelHeight / 2, levelLength / 2);

    leftWall.transform.position = new Vector3(-levelWidth / 2, levelHeight / 2, 0);
    rightWall.transform.position = new Vector3(levelWidth / 2, levelHeight / 2, 0);

    levelLight.transform.position = new Vector3(0, levelHeight, 0);

    // update the scales
    floor.transform.localScale = new Vector3(levelWidth, levelLength, 1);
    roof.transform.localScale = new Vector3(levelWidth, levelLength, 1);

    frontWall.transform.localScale = new Vector3(levelWidth, levelHeight, 1);
    backWall.transform.localScale = new Vector3(levelWidth, levelHeight, 1);

    leftWall.transform.localScale = new Vector3(levelLength, levelHeight, 1);
    rightWall.transform.localScale = new Vector3(levelLength, levelHeight, 1);
  }
}
