using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StepSize
{
  small = 20,
  medium = 10,
  large = 5
}

[System.Serializable]
public class PseudorandomChoiceSystem
{
  [Header("Chances Table")]
  public float[] chancesTable;

  [Header("Each timed Picked")]
  public int[] pickedTable;

  [Header("Settings")]
  public int numberOfOptions;
  public float step;

  [Header("Extra Info")]
  public int timesRun = 0;
  [Space(5)]
  public float currentPercent, averagePick;
  [Space(5)]
  public int  minTimesPicked = 0, maxTimesPicked = 0, diffrence = 0;


  public void InitilizeTables(int numberOfOptions, float step)
  {
    // set the variables
    this.numberOfOptions = numberOfOptions;
    this.step = step;
    currentPercent = 100;

    // initilize tables
    chancesTable = new float[this.numberOfOptions];
    pickedTable = new int[this.numberOfOptions];

    // starting number for each chance
    float startPercent = 100 / this.numberOfOptions;

    // set up the tables
    for (int i = 0; i < chancesTable.Length; i++)
    {
      chancesTable[i] = startPercent;
      pickedTable[i] = 0;
    }
  }

  public void InitilizeTables(int numberOfOptions, StepSize stepSize)
  {
    // set the variables
    this.numberOfOptions = numberOfOptions;
    currentPercent = 100;

    // initilize tables
    chancesTable = new float[this.numberOfOptions];
    pickedTable = new int[this.numberOfOptions];

    // starting number for each chance
    float startPercent = 100 / this.numberOfOptions;

    // set the step
    switch (stepSize)
    {
      case (StepSize.small):
        step = startPercent / (int)StepSize.small;
        break;

      case (StepSize.medium):
        step = startPercent / (int)StepSize.medium;
        break;
      case (StepSize.large):
        step = startPercent / (int)StepSize.large;
        break;
    }

    // set up the tables
    for (int i = 0; i < chancesTable.Length; i++)
    {
      chancesTable[i] = startPercent;
      pickedTable[i] = 0;
    }
  }

  public void GetChoice()
  {
    timesRun++;

    int choice = GetNumber();

    for (int i = 0; i < chancesTable.Length; i++)
    {
      if (i != choice)
      {
        chancesTable[i] += step / (chancesTable.Length - 1);
      }
      else
      {
        chancesTable[i] -= step;
        pickedTable[i]++;
      }
    }

    CheckPercentage();

    if (currentPercent != 100)
    {
      FixPercentage();
      CheckPercentage();
    }

    FindExtraInfo();
  }

  private void CheckPercentage()
  {
    float totalCost = 0;

    for (int i = 0; i < chancesTable.Length; i++)
    {
      totalCost += chancesTable[i];
    }

    currentPercent = totalCost;
  }
  private void FixPercentage()
  {
    chancesTable[UnityEngine.Random.Range(0, chancesTable.Length)] -= (currentPercent - 100);
  }

  private void FindExtraInfo()
  {
    minTimesPicked = BrandonsArrayFunctions.GetMin(pickedTable);
    maxTimesPicked = BrandonsArrayFunctions.GetMax(pickedTable);
    averagePick = BrandonsArrayFunctions.GetAverage(pickedTable);
    diffrence = maxTimesPicked - minTimesPicked;
  }

  private int GetNumber()
  {
    float costSoFar = 0;
    float randomNumber = UnityEngine.Random.Range(0F, currentPercent);

    for (int i = 0; i < chancesTable.Length; i++)
    {
      costSoFar += chancesTable[i];

      if (randomNumber <= costSoFar && costSoFar != 0)
      {
        return i;
      }
    }

    return -1;
  }
}
