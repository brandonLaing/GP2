using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class BrandonsArrayFunctions
{
  // gets the min from a comparable array
  public static T GetMin<T>(T[] array) where T : IComparable
  {
    if (array == null)
    {
      throw new Exception("Array cannot be null");
    }
    if (array.Length == 0)
    {
      throw new Exception("Array must have one or more objects");
    }

    T min = array[0];

    for (int i = 0; i < array.Length; i++)
    {
      if (min.CompareTo(array[i]) > 0)
      {
        min = array[i];
      }
    }

    return min;
  }

  // gets the max from a comparable array
  public static T GetMax<T>(T[] array) where T : IComparable
  {
    if (array == null)
    {
      throw new Exception("Array cannot be null");
    }
    if (array.Length == 0)
    {
      throw new Exception("Array must have one or more objects");
    }

    T max = array[0];

    for (int i = 0; i < array.Length; i++)
    {
      if (max.CompareTo(array[i]) < 0)
      {
        max = array[i];
      }
    }

    return max;
  }

  // finds the average for a float array
  public static float GetAverage<T>(T[] array) where T: IConvertible
  {
    if (array == null)
    {
      throw new Exception("Array cannot be null");
    }
    if (array.Length == 0)
    {
      throw new Exception("Array must have one or more objects");
    }

    float total = 0;

    for (int i = 0; i < array.Length; i++)
    {
      total += Convert.ToSingle(array[i]);
    }

    return total / array.Length;
  }
}