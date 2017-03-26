using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class ApplyShader : MonoBehaviour {

    public Shader shader;
    public Camera otherCamera;
    public RenderTexture tex;
    public Material mat;
    [Range(0.0f,1.0f)]
    public float mix = .5f;

    public GameObject go;
    public RenderToRT rto;

    Camera cam;

    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
        tex = new RenderTexture(otherCamera.pixelWidth,otherCamera.pixelHeight,24);
        rto.rt = tex;
        
        //otherCamera.targetTexture = tex;
        //mat = new Material(shader);
        mat.SetTexture("_RenderTex",tex);
               

        //otherCamera.transform.parent = transform;
        //otherCamera.transform.localPosition = Vector3.zero;
        // + SteamVR.instance.eyes[0].pos;
        //otherCamera.transform.localRotation = Quaternion.identity;

        if (go) {
            go.GetComponent<Renderer>().sharedMaterial.mainTexture = tex;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(mat)
            mat.SetFloat("_Mix", mix);
    }

    void OnRenderImage(RenderTexture source, RenderTexture dest) {
        Graphics.Blit(source,dest,mat);
    }    
}
