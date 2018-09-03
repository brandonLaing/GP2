using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Random Cube Setter Does:
 * On called this will make cubes that fit the detentions of the board
 */
public class RandonCubeSetter : MonoBehaviour
{
  public TileGenerator tileGen;

  public GameObject cubePrefab;

  public int numberOfCubes = 0;

  private List<GameObject> allCubes = new List<GameObject>();

  // for as many cubes as you want it will get a random position but if its too close to another cube it doesn't make it.
  // once it knows its in a good spot it makes the cube
  public void MakeCubes()
  {
    for (int i = 0; i < numberOfCubes;)
    {
      bool makeCube = true;

      Vector3 cubePosition = GetRandomCubePosition();

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

  // gets a position between 1 and the max height and 1 and max width
  private Vector3 GetRandomCubePosition()
  {
    float randomHeight = Random.Range(1, tileGen.gridHeight - 1);
    float randomWidth = Random.Range(1, tileGen.gridWidth - 1);

    Vector3 cubePosition = new Vector3(randomWidth, 0, randomHeight);

    return cubePosition;
  }

}
