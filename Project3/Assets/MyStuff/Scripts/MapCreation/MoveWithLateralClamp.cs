using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithLateralClamp : MonoBehaviour {

  public float moveSpeed;
  float minX = -10, maxX = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    if (Input.GetKey(KeyCode.LeftArrow))
    {
      transform.position += Time.deltaTime * moveSpeed * Vector3.left;
    }
    if (Input.GetKey(KeyCode.RightArrow))
    {
      transform.position += Time.deltaTime * moveSpeed * Vector3.right;
    }

    Vector3 newPos = transform.position;
    newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
    transform.position = newPos;
  }
}
