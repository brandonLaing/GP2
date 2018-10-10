using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffballController : MonoBehaviour
{
  // keep track of owning player
  public PlayerScore owningPlayer;

  [Header("Destruction")]
  public bool destroyPuffBall = true;
  public float liveTime = 20F;
  public float sinkTime = 1F;

  private void Awake()
  {
    if (destroyPuffBall)
    {
      StartCoroutine(LiveTimer());
    }
  }
  
  IEnumerator LiveTimer()
  {
    yield return new WaitForSecondsRealtime(liveTime);
    StartCoroutine(SinkTimer());
  }

  IEnumerator SinkTimer()
  {
    GetComponent<SphereCollider>().isTrigger = true;
    yield return new WaitForSeconds(sinkTime);
    Destroy(gameObject);
  }
}
