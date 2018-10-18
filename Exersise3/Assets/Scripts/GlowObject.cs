using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowObject : MonoBehaviour
{
  public Color glowColor;
  public float lerpFactor = 10;

  public Renderer[] Renderers
  {
    get;
    private set;
  }

  public Color CurrentColor
  {
    get { return currentColor; }
  }

  private List<Material> materials = new List<Material>();
  private Color currentColor;
  private Color targetColor;

  private void Start()
  {
    Renderers = GetComponentsInChildren<Renderer>();

    foreach (var renderer in Renderers)
    {
      materials.AddRange(renderer.materials);
    }
  }

  private void OnMouseEnter()
  {
    targetColor = glowColor;
    enabled = true;
  }

  private void OnMouseExit()
  {
    targetColor = Color.black;
    enabled = true;
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
      enabled = false;
    }
	}
}
