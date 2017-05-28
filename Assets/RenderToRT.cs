using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderToRT : MonoBehaviour {    
    public RenderTexture renderTex;
    void OnRenderImage(RenderTexture source, RenderTexture dest) {
        if (renderTex) {
            Graphics.Blit(source,renderTex);
        }
    }
}
