using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class ApplyShader : MonoBehaviour {

    [Range(0.0f,1.0f)]
    public float mix = 0f;
    
    public GameObject[] debugObjectsA, debugObjectsB;

    public int defaultLayer = 0;
    public int layerForFadeIns = 2;
    public int layerForFadeOuts = 3;

    private Shader shader;
    private RenderToRT otherRenderToRT;
    private Material mat;
    private Camera cam;
    private Camera otherCam;
    private GameObject other;

    private GameObject[] fadeIns, fadeOuts;

    private bool fade = false;

    enum State {
        Null,
        FadeIn
    }
    private State state = State.Null;

    void Start () {       

        cam = GetComponent<Camera>();
        shader = Shader.Find("Custom/CameraMix");

        transform.parent = null;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        other = new GameObject("Other Camera");
        other.transform.parent = null;
        other.transform.localPosition = Vector3.zero;
        other.transform.localRotation = Quaternion.identity;

        otherCam = other.AddComponent<Camera>();
        otherCam.cullingMask = 0;

        otherRenderToRT = other.AddComponent<RenderToRT>();
        otherRenderToRT.renderTex = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 24);

        mat = new Material(shader);
        mat.SetTexture("_RenderTex", otherRenderToRT.renderTex);
    }

	void Update () {
        if (mat) {
            switch (state) {
                case State.FadeIn: {
                        if(mix < 1.0f) {
                            mix += Time.deltaTime;
                        } else {
                            foreach (GameObject g in fadeIns) {
                                g.layer = defaultLayer;
                            }

                            foreach (GameObject g in fadeOuts) {
                                g.SetActive(false);
                                g.layer = defaultLayer;
                            }
                            cam.cullingMask = (1 << defaultLayer);
                            otherCam.cullingMask = 0;
                            mix = 0f;
                            state = State.Null;
                        }
                        break;
                    }
                case State.Null: {

                        // Debugging Key
                        if (Input.GetKeyDown(KeyCode.A)) {
                            fade = !fade;
                            if (fade) {
                                FadeGameObjects(debugObjectsA, debugObjectsB);
                            } else {
                                FadeGameObjects(debugObjectsB, debugObjectsA);
                            }
                        }
                        break;
                    }
            }
            mat.SetFloat("_Mix", mix);
        }
    }

    public void FadeGameObjects(GameObject[] fadeOuts, GameObject[] fadeIns) {
        this.fadeOuts = fadeOuts;
        this.fadeIns = fadeIns;

        cam.cullingMask = (1 << defaultLayer) | (1 << layerForFadeOuts);
        otherCam.cullingMask = (1 << defaultLayer) | (1 << layerForFadeIns);

        foreach(GameObject g in fadeIns) {
            g.SetActive(true);
            g.layer = layerForFadeIns;
        }

        foreach (GameObject g in fadeOuts) {
            g.layer = layerForFadeOuts;
        }

        mix = 0f;
        state = State.FadeIn;
    }

    void OnRenderImage(RenderTexture source, RenderTexture dest) {
        Graphics.Blit(source,dest,mat);
    }    
}
