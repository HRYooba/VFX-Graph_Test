using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class WebCameraEffect : MonoBehaviour
{
    int m_width = 640;
    int m_height = 480;
    int m_fps = 30;

    WebCamTexture m_webcamTex;
    Texture2D m_webcamTexBuffer;
    Material m_subtMat;
    Material m_sortMat;

    [SerializeField] Shader m_subtShader;
    [SerializeField] Shader m_sortShader;
    [SerializeField] CustomRenderTexture m_subtTexture;
    [SerializeField] CustomRenderTexture m_sortTexture;
    [SerializeField] VisualEffect m_vfx;
    [SerializeField] InfraredSourceManager m_infrared;
    [SerializeField] ColorSourceManager m_color;



    // Start is called before the first frame update
    void Start()
    {
        m_subtMat = new Material(m_subtShader);
        m_subtTexture.material = m_subtMat;

        m_sortMat = new Material(m_sortShader);
        m_sortTexture.material = m_sortMat;

        if (m_infrared == null || m_color == null)
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            m_webcamTex = new WebCamTexture(devices[0].name, m_width, m_height, m_fps);
            m_webcamTex.Play();
            StartCoroutine(UpdateWebcamTex());
        }
        else
        {
            StartCoroutine(UpdateKinectTex());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator UpdateWebcamTex()
    {
        while (m_webcamTex.width <= 16 || m_webcamTex.height <= 16)
        {
            yield return null;
        }

        m_webcamTexBuffer = new Texture2D(m_webcamTex.width, m_webcamTex.height);

        m_vfx.SetTexture("ColorTex", m_webcamTex);
        m_vfx.SetTexture("ResultTex", m_sortTexture);

        m_subtMat.SetTexture("_MainTex", m_webcamTex);
        m_subtMat.SetTexture("_BufferTex", m_webcamTexBuffer);

        m_sortMat.SetTexture("_MainTex", m_subtTexture);

        Debug.Log("Start Webcam: (" + m_webcamTex.width + ", " + m_webcamTex.height + ")");

        while (true)
        {
            m_webcamTexBuffer.SetPixels(m_webcamTex.GetPixels());
            yield return null;
            m_webcamTexBuffer.Apply();
        }
    }

    IEnumerator UpdateKinectTex()
    {
        m_vfx.SetTexture("ResultTex", m_sortTexture);
        m_vfx.SetTexture("ColorTex", m_color.GetColorTexture());
        m_sortMat.SetTexture("_MainTex", m_subtTexture);

        while (true)
        {
            m_subtMat.SetTexture("_MainTex", m_color.GetColorTexture());
            m_subtMat.SetTexture("_BufferTex", m_infrared.GetInfraredTexture());
            yield return null;
        }
    }
}
