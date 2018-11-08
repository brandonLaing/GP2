using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AxisType
{
  xAxis,
  yAxis,
  zAxis
}

public enum TransformationType
{
  position,
  rotation,
  scale
}
public class LevelObjectPlacementEditing : MonoBehaviour
{


  public LevelObjectsEditing objectEditor;

  public InputField[] positionFields;
  [Space(10)]
  public InputField[] rotationFields;
  [Space(10)]
  public InputField[] scaleFields;
  [Space(10)]

  public AxisType currentAxis = AxisType.xAxis;
  public TransformationType currentTrasformation = TransformationType.position;

  private Vector3 startMousePosition = new Vector3();
  private Vector3 currentMouseDiffrence = new Vector3();

  public float mouseControlsDeadZone = 0.2F;
  public float transformSpeed;

  private void Start()
  {
    objectEditor = GetComponentInParent<LevelObjectsEditing>();
  }

  private void Update()
  {
    CheckToChangeTransformation();
    CheckToChangeAxis();
    CheckToAdjustObject();
  }

  private void CheckToChangeAxis()
  {
    if (Input.GetKeyDown(KeyCode.Z) && !Input.GetMouseButton(1))
    {
      currentAxis = AxisType.xAxis;
    }
    if (Input.GetKeyDown(KeyCode.X) && !Input.GetMouseButton(1))
    {
      currentAxis = AxisType.yAxis;
    }
    if (Input.GetKeyDown(KeyCode.C) && !Input.GetMouseButton(1))
    {
      currentAxis = AxisType.zAxis;
    }
  }

  private void CheckToChangeTransformation()
  {
    if (Input.GetKeyDown(KeyCode.Q) && !Input.GetMouseButton(1))
    {
      currentTrasformation = TransformationType.position;
    }
    if (Input.GetKeyDown(KeyCode.W) && !Input.GetMouseButton(1))
    {
      currentTrasformation = TransformationType.rotation;
    }
    if (Input.GetKeyDown(KeyCode.E) && !Input.GetMouseButton(1))
    {
      currentTrasformation = TransformationType.scale;
    }
  }

  // this looks really bad and needs some exporting to diffrent methods but i dont want to do that right now
  private void CheckToAdjustObject()
  {
    if (Input.GetMouseButtonDown(0))
    {
      startMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
      return;
    }

    if (Input.GetMouseButton(0) && objectEditor.targetObject != null)
    {
      switch (currentTrasformation)
      {
        case TransformationType.position:
          switch (currentAxis)
          {
            case AxisType.xAxis:
              currentMouseDiffrence = startMousePosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);

              if (currentMouseDiffrence.x > mouseControlsDeadZone || currentMouseDiffrence.x < -mouseControlsDeadZone)
              {
                currentMouseDiffrence.y = 0;
                currentMouseDiffrence.z = 0;

                objectEditor.targetObject.transform.position += currentMouseDiffrence * transformSpeed * Time.deltaTime;
                SetAllInputs();
              }
              break;

            case AxisType.yAxis:
              currentMouseDiffrence = startMousePosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);

              if (currentMouseDiffrence.x > mouseControlsDeadZone || currentMouseDiffrence.x < -mouseControlsDeadZone)
              {
                currentMouseDiffrence.y = currentMouseDiffrence.x;
                currentMouseDiffrence.x = 0;
                currentMouseDiffrence.z = 0;

                objectEditor.targetObject.transform.position += currentMouseDiffrence * transformSpeed * Time.deltaTime;
                SetAllInputs();
              }
              break;

            case AxisType.zAxis:
              currentMouseDiffrence = startMousePosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);

              if (currentMouseDiffrence.x > mouseControlsDeadZone || currentMouseDiffrence.x < -mouseControlsDeadZone)
              {
                currentMouseDiffrence.z = currentMouseDiffrence.x;
                currentMouseDiffrence.x = 0;
                currentMouseDiffrence.y = 0;

                objectEditor.targetObject.transform.position += currentMouseDiffrence * transformSpeed * Time.deltaTime;
                SetAllInputs();
              }
              break;
          }
          break;

        case TransformationType.rotation:
          switch (currentAxis)
          {
            case AxisType.xAxis:
              currentMouseDiffrence = startMousePosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);

              if (currentMouseDiffrence.x > mouseControlsDeadZone || currentMouseDiffrence.x < -mouseControlsDeadZone)
              {
                currentMouseDiffrence.y = 0;
                currentMouseDiffrence.z = 0;

                objectEditor.targetObject.transform.eulerAngles += currentMouseDiffrence * transformSpeed * Time.deltaTime;
                SetAllInputs();
              }
              break;

            case AxisType.yAxis:
              currentMouseDiffrence = startMousePosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);

              if (currentMouseDiffrence.x > mouseControlsDeadZone || currentMouseDiffrence.x < -mouseControlsDeadZone)
              {
                currentMouseDiffrence.y = currentMouseDiffrence.x;
                currentMouseDiffrence.x = 0;
                currentMouseDiffrence.z = 0;

                objectEditor.targetObject.transform.eulerAngles += currentMouseDiffrence * transformSpeed * Time.deltaTime;
                SetAllInputs();
              }
              break;

            case AxisType.zAxis:
              currentMouseDiffrence = startMousePosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);

              if (currentMouseDiffrence.x > mouseControlsDeadZone || currentMouseDiffrence.x < -mouseControlsDeadZone)
              {
                currentMouseDiffrence.z = currentMouseDiffrence.x;
                currentMouseDiffrence.x = 0;
                currentMouseDiffrence.y = 0;

                objectEditor.targetObject.transform.eulerAngles += currentMouseDiffrence * transformSpeed * Time.deltaTime;
                SetAllInputs();
              }
              break;
          }
          break;

        case TransformationType.scale:
          switch (currentAxis)
          {
            case AxisType.xAxis:
              currentMouseDiffrence = startMousePosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);

              if (currentMouseDiffrence.x > mouseControlsDeadZone || currentMouseDiffrence.x < -mouseControlsDeadZone)
              {
                currentMouseDiffrence.y = 0;
                currentMouseDiffrence.z = 0;

                objectEditor.targetObject.transform.localScale += currentMouseDiffrence * transformSpeed * Time.deltaTime;
                SetAllInputs();
              }
              break;

            case AxisType.yAxis:
              currentMouseDiffrence = startMousePosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);

              if (currentMouseDiffrence.x > mouseControlsDeadZone || currentMouseDiffrence.x < -mouseControlsDeadZone)
              {
                currentMouseDiffrence.y = currentMouseDiffrence.x;
                currentMouseDiffrence.x = 0;
                currentMouseDiffrence.z = 0;

                objectEditor.targetObject.transform.localScale += currentMouseDiffrence * transformSpeed * Time.deltaTime;
                SetAllInputs();
              }
              break;

            case AxisType.zAxis:
              currentMouseDiffrence = startMousePosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);

              if (currentMouseDiffrence.x > mouseControlsDeadZone || currentMouseDiffrence.x < -mouseControlsDeadZone)
              {
                currentMouseDiffrence.z = currentMouseDiffrence.x;
                currentMouseDiffrence.x = 0;
                currentMouseDiffrence.y = 0;

                objectEditor.targetObject.transform.localScale += currentMouseDiffrence * transformSpeed * Time.deltaTime;
                SetAllInputs();
              }
              break;
          }
          break;
      }
    }
  }

  // updates ui with objects variables
  public void SetAllInputs()
  {
    objectEditor.targetObject.CheckObjectConstraints();

    positionFields[0].text = objectEditor.targetObject.transform.position.x.ToString(); ;
    positionFields[1].text = objectEditor.targetObject.transform.position.y.ToString(); ;
    positionFields[2].text = objectEditor.targetObject.transform.position.z.ToString(); ;

    rotationFields[0].text = objectEditor.targetObject.transform.eulerAngles.x.ToString(); ;
    rotationFields[1].text = objectEditor.targetObject.transform.eulerAngles.y.ToString(); ;
    rotationFields[2].text = objectEditor.targetObject.transform.eulerAngles.z.ToString(); ;

    scaleFields[0].text = objectEditor.targetObject.transform.localScale.x.ToString(); ;
    scaleFields[1].text = objectEditor.targetObject.transform.localScale.y.ToString(); ;
    scaleFields[2].text = objectEditor.targetObject.transform.localScale.z.ToString(); ;
  }

  public void UpdatePositionInputs()
  {
    Vector3 adjustedVector = new Vector3();
    UpdateInputs(positionFields, out adjustedVector);

    objectEditor.targetObject.transform.position = adjustedVector;
    objectEditor.targetObject.CheckObjectConstraints();
  }

  public void UpdateRotationInputs()
  {
    Vector3 adjustedVector = new Vector3();
    UpdateInputs(rotationFields, out adjustedVector);

    objectEditor.targetObject.transform.eulerAngles = adjustedVector;
    objectEditor.targetObject.CheckObjectConstraints();
  }

  public void UpdateScaleInputs()
  {
    Vector3 adjustedVector = new Vector3();
    UpdateInputs(scaleFields, out adjustedVector);

    objectEditor.targetObject.transform.localScale = adjustedVector;
    objectEditor.targetObject.CheckObjectConstraints();
  }

  private void UpdateInputs(InputField[] inputArray, out Vector3 newVector)
  {
    newVector = new Vector3();

    float.TryParse(inputArray[0].text, out newVector.x);
    float.TryParse(inputArray[1].text, out newVector.y);
    float.TryParse(inputArray[2].text, out newVector.z);

    inputArray[0].text = newVector.x.ToString();
    inputArray[1].text = newVector.y.ToString();
    inputArray[2].text = newVector.z.ToString();
  }

}
