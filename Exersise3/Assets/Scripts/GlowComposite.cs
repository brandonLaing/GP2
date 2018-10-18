using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class GlowComposite : MonoBehaviour
{
  [Range(0, 10)]
  public float Intensity = 2;

  private Material compositMat;

  private void OnEnable()
  {
    compositMat = new Material(Shader.Find("Hidden/GlowComposite"));

  }

  private void OnRenderImage(RenderTexture src, RenderTexture dst)
  {
    compositMat.SetFloat("Intensity", Intensity);
    Graphics.Blit(src, dst, compositMat, 0);

  }
}
