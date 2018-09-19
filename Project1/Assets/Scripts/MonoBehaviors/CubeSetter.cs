using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSetter : MonoBehaviour
{
  public GameObject cubePrefab;

  public int numberOfCubes;

  public void SetCubes(int gridWidth, int gridHeight)
  {
    List<GameObject> allCubes = new List<GameObject>();

    int j = 0;
    for (int i = 0; i < numberOfCubes && j < numberOfCubes * 10; j++)
    {
      Vector3 cubePosition = GetRandomCubePosition(gridWidth, gridHeight);
      bool makeCube = true;

      foreach (GameObject cube in allCubes)
      {
        if (Vector3.Distance(cubePosition, cube.transform.position) <= 1.5F)
        {
          makeCube = false;
        }
      }

      if (makeCube)
      {
        GameObject newCube = Instantiate(cubePrefab, cubePosition, cubePrefab.transform.rotation, this.transform);

        allCubes.Add(newCube);

        i++;

      }
    }
  }

  private Vector3 GetRandomCubePosition(int width, int height)
  {
    float randomWidth = UnityEngine.Random.Range(1, width - 1);
    float randomHeight = UnityEngine.Random.Range(1, height - 1);

    Vector3 cubePosition = new Vector3(randomWidth, 0, randomHeight);

    return cubePosition;

  }
}
