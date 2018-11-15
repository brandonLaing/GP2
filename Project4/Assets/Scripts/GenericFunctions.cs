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

  public static void SortBubble<T>(ref T[] array) where T : IComparable
  {
    for (int i = 0; i <= array.Length - 2; i++)
    {
      for (int j = 0; j <= array.Length - 2; j++)
      {
        if (array[j].CompareTo(array[j + 1]) > 0)
        {
          T temp = array[j + 1]; ;
          array[j + 1] = array[j];
          array[j] = temp;
        }
      }
    }
  }

  public static void Reverse<T>(ref T[] array)
  {
    T[] temp = new T[array.Length];
    
    for (int i = array.Length; i > 0; i--)
    {
      temp
    }
  }
}