using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffballController : MonoBehaviour
{
  public PlayerScore owningPlayer;
  public bool destroyPuffBall = true;
  private float liveTime = 20F;
  private float sinkTime = 1F;

  private void Start()
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
