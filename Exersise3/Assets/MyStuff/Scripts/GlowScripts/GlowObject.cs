using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GlowObject : MonoBehaviour, IGlowable, IInteractable
{
  public Color glowColor;
  public float lerpFactor = 10;

  public bool found;

  public Renderer[] Renderers
  {
    get;
    private set;
  }

  private List<Material> materials = new List<Material>();
  private Color currentColor;
  private Color targetColor;

  private DateTime startTime;

  private void Start()
  {
    Renderers = GetComponentsInChildren<Renderer>();
    startTime = DateTime.Now;

    foreach (var renderer in Renderers)
    {
      materials.AddRange(renderer.materials);
    }
  }

  public void Glow(bool shouldGlow)
  {
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

  public void Interact()
  {
    if (!found)
    {
      Glow(false);
      GetComponentInParent<GameController>().chosenGlows.Remove(this.gameObject);
      SendAnalyticsEvent();
      found = true;
    }
  }

  private void SendAnalyticsEvent()
  {
    TimeSpan timeToFind = DateTime.Now.Subtract(startTime);
    AnalyticsEvent.Custom("object_found", new Dictionary<string, object>
    {
      { "object_name", transform.name},
      { "time_taken_seconds", timeToFind.Seconds }
    });
  }

  void Update()
  {
    currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * lerpFactor);

    for (int i = 0; i < materials.Count; i++)
    {
      materials[i].SetColor("_GlowColor", currentColor);

    }

    if (currentColor.Equals(targetColor))
    {
      if (found)
      {
        Destroy(this);

      }
    }
	}
}
