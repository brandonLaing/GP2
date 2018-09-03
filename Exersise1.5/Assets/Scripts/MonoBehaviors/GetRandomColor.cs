using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRandomColor : MonoBehaviour
{
	void Start ()
  {
    this.transform.gameObject.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
  }
}
