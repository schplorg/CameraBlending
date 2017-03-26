using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RenderToRT : MonoBehaviour {

    Camera cam;
    public RenderTexture rt;
    
	void Start () {
        cam = GetComponent<Camera>();
        rt = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 24);
    }
	
	void Update () {
		
	}

    void OnRenderImage(RenderTexture source, RenderTexture dest) {
        if (rt) {
            Graphics.Blit(source,rt);
        }
    }
}
