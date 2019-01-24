using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;


public class WebCamera : MonoBehaviour
{
    int width = 640;
    int height = 480;
    int fps = 30;
    WebCamTexture webcamTexture;
    public VisualEffect vfx;


    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[0].name, this.width, this.height, this.fps);
        webcamTexture.Play();
        vfx.SetTexture("WebCamera", webcamTexture);
    }

    void Update()
    {
        // Debug.Log(webcamTexture.width + "," + webcamTexture.height);
    }
}
