using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudorandomChoiceDebugger : MonoBehaviour
{
  [Tooltip("Number of options you want to be able to choose from.")]
  public int numberOfOptions;

  [Header("Diffrence options")]
  [Tooltip("How much diffrence is made each time the simulation is run")]
  public float step;
  public bool setStep = false;

  [Header("StepSize")]
  public StepSize stepSize;

  [Header("System Info")]
  public PseudorandomChoiceSystem pseudorandomSystem = new PseudorandomChoiceSystem();

  public List<Vector3> inputed = new List<Vector3>();
  public List<Vector3> outpued = new List<Vector3>();

  private void Start()
  {
    if (setStep)
    {
      pseudorandomSystem.InitilizeTables(numberOfOptions, step);
    }
    else
    {
      pseudorandomSystem.InitilizeTables(numberOfOptions, stepSize);
    }
  }

  private void Update()
  {
    // fast sim
    if (Input.GetKey(KeyCode.KeypadEnter) && Input.GetKey(KeyCode.LeftShift))
    {
      pseudorandomSystem.GetChoice();
    }

    // single sim
    if (Input.GetKeyDown(KeyCode.KeypadEnter) && !Input.GetKey(KeyCode.LeftShift))
    {
      pseudorandomSystem.GetChoice();
    }
  }

  public void RunSystem()
  {
    pseudorandomSystem.GetChoice();
  }
}