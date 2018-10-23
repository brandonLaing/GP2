using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementControls : MonoBehaviour {

  public float moveSpeed;

  void Update()
  {
    MoveCharecter();

  }

  void MoveCharecter()
  {
    Vector3 moveDirection = new Vector3();

    if (Input.GetKey(KeyCode.W)) moveDirection += transform.forward;
    if (Input.GetKey(KeyCode.S)) moveDirection -= transform.forward;
    if (Input.GetKey(KeyCode.D)) moveDirection += transform.right;
    if (Input.GetKey(KeyCode.A)) moveDirection -= transform.right;

    transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
  }
}
