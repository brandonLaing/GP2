using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  public List<GameObject> allGlowables = new List<GameObject>();

  public List<GameObject> chosenGlows = new List<GameObject>();

  [Range(0, 5)]
  public int objectsToGlow = 2;

  private void Start()
  {
    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Props"))
    {
      allGlowables.Add(obj);
    }

    for (int i = 0; i < objectsToGlow && allGlowables.Count > 0; i++)
    {
      ChooseObjectToGlow();

    }

    foreach (GameObject obj in chosenGlows)
    {
      AddGlows(obj);
    }
  }

  private void AddGlows(GameObject obj)
  {
    GlowObject go = obj.AddComponent<GlowObject>();
    go.glowColor = Random.ColorHSV(0, 1, 0, 1, 0, 1, 0, 0);
    go.Glow(false);

  }

  private void ChooseObjectToGlow()
  {
    int randomInt = Random.Range(0, allGlowables.Count);

    chosenGlows.Add(allGlowables[randomInt]);
    allGlowables.RemoveAt(randomInt);

  }
}
