using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class GlowComposite : MonoBehaviour
{
  [Range(0, 10)]
  public float Intensity = 2;

  public Shader glowCompositeShader;

  private Material compositMat;

  private void Start()
  {
    Debug.Log("GlowComposite started");
  }

  private void OnEnable()
  {
    Debug.Log("GlowComposite enabled");
    
    compositMat = new Material(glowCompositeShader);



  }

  private void OnRenderImage(RenderTexture src, RenderTexture dst)
  {
    if (src == null) Debug.Log("GlowComposite.OnRenderImage - src is null"); else Debug.Log("GlowComposite.OnRenderImage - src ok");
    if (dst == null) Debug.Log("GlowComposite.OnRenderImage - dst is null"); else Debug.Log("GlowComposite.OnRenderImage - dst ok");
    if (compositMat == null) Debug.Log("GlowComposite.OnRenderImage - compositMat is null"); else Debug.Log("GlowComposite.OnRenderImage - compositMat ok");

    compositMat.SetFloat("Intensity", Intensity);
    Graphics.Blit(src, dst, compositMat, 0);

  }
}
