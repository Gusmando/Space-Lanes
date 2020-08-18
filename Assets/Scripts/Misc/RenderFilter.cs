using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderFilter : MonoBehaviour
{
    [Header("Intensity Level (0-3)")]
    public int intensityLevel;

    [Header("Camera")]
    public Camera playerCam;

    [Header("Render Texture")]
    public RawImage currentResolution;
    
    [Header("Resolution Options")]
    public RenderTexture full;
    public RenderTexture high;
    public RenderTexture mid;
    public RenderTexture low;
    void Update()
    {
        switch (intensityLevel)
      {
          case 0:
              currentResolution.texture = full;
              playerCam.targetTexture = full;
              break;
          case 1:
              currentResolution.texture = high;
              playerCam.targetTexture = high;
              break;
          case 2:
              currentResolution.texture = mid;
              playerCam.targetTexture = mid;
              break;
          case 3:
              currentResolution.texture = low;
              playerCam.targetTexture = low;
              break;
      }

    }

    public void setIntensity(int level)
    {
       intensityLevel = level;
    }
}
