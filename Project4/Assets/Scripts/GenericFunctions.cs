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

  // sorts array using bubble method
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

  // reverses array
  public static void Reverse<T>(ref T[] array)
  {
    T[] temp = new T[array.Length];

    for (int i = array.Length - 1, j = 0; i >= 0; i--, j++)
    {
      temp[j] = array[i];
    }

    array = temp;
  }
}

// que i made
public class BrandonsQue<T>
{
  private List<T> _que = new List<T>();

  public int Count { get { return _que.Count; } }

  // adds
  public void Push(T newObject)
  {
    _que.Add(newObject);
  }

  // removes
  public T Pop()
  {
    if (Count > 0)
    {
      T temp;
      temp = _que[0];
      _que.RemoveAt(0);
      return temp;
    }

    throw new InvalidOperationException("Operation not valid due to the current state of the object");
  }

  public void Clear()
  {
    _que = new List<T>();
  }

  public T PeekNext()
  {
    if (Count > 0)
    {
      return _que[0];
    }

    throw new InvalidOperationException("Operation not valid due to the current state of the object");
  }

  public override string ToString()
  {
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    
    foreach (T item in _que)
    {
      sb.AppendLine(item.ToString());
    }

    return sb.ToString();
  }
}

public class BrandonsStack<T>
{
  private List<T> _stack = new List<T>();

  public int Count { get { return _stack.Count; } }

  public void Push (T item)
  {
    _stack.Insert(0, item);
  }

  public T Pop()
  {
    if (Count > 0)
    {
      T temp;
      temp = _stack[0];
      _stack.RemoveAt(0);
      return temp;
    }

    throw new InvalidOperationException("Operation not valid due to the current state of the object");
  }

  public void Clear()
  {
    _stack = new List<T>();
  }
}
