using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CubeRandomMatPicker : MonoBehaviour
{
  public Material[] randomMaterials;
  private MeshRenderer myMesh;
  private void Start()
  {
    myMesh = GetComponent<MeshRenderer>();
    int randomNumber = Random.Range(0, randomMaterials.Length);
    myMesh.material = randomMaterials[randomNumber];
  }
}
