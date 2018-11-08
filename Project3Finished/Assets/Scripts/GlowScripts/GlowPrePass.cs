using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class GlowPrePass : MonoBehaviour
{
  private static RenderTexture prePass;
  private static RenderTexture Blurred;

  private Material blurMat;

  private void OnEnable()
  {
    prePass = new RenderTexture(Screen.width, Screen.height, 24)
    {
      antiAliasing = QualitySettings.antiAliasing
    };
    Blurred = new RenderTexture(Screen.width >> 1, Screen.height >> 1, 0);

    var camera = GetComponent<Camera>();
    var glowShader = Shader.Find("Hidden/GlowReplace");
    camera.targetTexture = prePass;
    camera.SetReplacementShader(glowShader, "Glowable");
    Shader.SetGlobalTexture("_GlowPrePassTex", prePass);

    Shader.SetGlobalTexture("_GlowBlurredTex", Blurred);

    blurMat = new Material(Shader.Find("Hidden/Blur"));
    blurMat.SetVector("_BlurSize", new Vector2(Blurred.texelSize.x * 1.5f, Blurred.texelSize.y * 1.5f));
  }

  void OnRenderImage(RenderTexture src, RenderTexture dst)
  {
    Graphics.Blit(src, dst);

    Graphics.SetRenderTarget(Blurred);
    GL.Clear(false, true, Color.clear);

    Graphics.Blit(src, Blurred);

    for (int i = 0; i < 4; i++)
    {
      var temp = RenderTexture.GetTemporary(Blurred.width, Blurred.height);
      Graphics.Blit(Blurred, temp, blurMat, 0);
      Graphics.Blit(temp, Blurred, blurMat, 1);
      RenderTexture.ReleaseTemporary(temp);
    }
  }
}
