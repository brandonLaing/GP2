using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GlowObject : MonoBehaviour, IGlowable, IInteractable
{
  [Header("Glow Settings")]
  public Color glowColor;
  public float lerpFactor = 10;

  // if the object has been found
  private bool found;

  public Renderer[] Renderers
  {
    get;
    private set;
  }

  private List<Material> materials = new List<Material>();
  private Color currentColor;
  private Color targetColor;

  // start time of the game
  private DateTime startTime;

  private void Start()
  {
    // set the start time
    startTime = DateTime.Now;

    // get the renderers
    Renderers = GetComponentsInChildren<Renderer>();
    foreach (var renderer in Renderers)
    {
      materials.AddRange(renderer.materials);
    }
  }


  void Update()
  {
    // lerp towards our current color
    currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * lerpFactor);

    // this is for the shader
    for (int i = 0; i < materials.Count; i++)
    {
      materials[i].SetColor("_GlowColor", currentColor);
    }

    // if found destroy this script, we cant destroy before this cause we need all of our other materials to set the glow color
    if (found)
    {
      Destroy(this);
    }
  }


  // this tell the object when to glow
  public void Glow(bool shouldGlow)
  {
    // it be affected if its already been found
    if (!found)
    {
      if (shouldGlow)
      {
        targetColor = glowColor;
      }
      else
      {
        targetColor = Color.black;
      }
    }
  }


  // you can only interact with a object once and after the first time its set to found
  public void Interact()
  { 
    if (!found)
    {
      // set this object as found
      found = true;

      // send a message to our gamecontroller that the object was found
      TimeSpan timeToFind = DateTime.Now.Subtract(startTime);
      GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSceneController>().ObjectFound(gameObject, timeToFind);

      // send our own event that we were found
      SendAnalyticsEvent(timeToFind);

      // set the color to black and also set our target color to black within the glow function
      currentColor = Color.black;
      Glow(false);

    }
  }


  // Send info on our object and how long it took to find us
  private void SendAnalyticsEvent(TimeSpan timeToFind)
  {
    //AnalyticsEvent.Custom("object_found", new Dictionary<string, object>
    //{
    //  { "object_name", transform.name},
    //  { "time_taken_seconds", timeToFind.Seconds }
    //});
  }
}
