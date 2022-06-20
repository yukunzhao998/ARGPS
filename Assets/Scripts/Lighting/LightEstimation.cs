using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class LightEstimation : MonoBehaviour
{
    private ARCameraManager m_CameraManager;
    private Light m_Light;

    private void Awake()
    {
        m_CameraManager = FindObjectOfType<ARCameraManager>();
        if(m_CameraManager==null)
        {
            Debug.LogError(GetType() + "/Awake()/ ARCameraManager is null!");
        }

        m_Light = GetComponent<Light>();
    }

    private void OnEnable()
    {
        if (m_CameraManager != null)
        {
            m_CameraManager.frameReceived += OnFrameChanged;
        }
    }

    private void OnDisable()
    {
        if (m_CameraManager != null)
        {
            m_CameraManager.frameReceived -= OnFrameChanged;
        }
    }

    private void OnFrameChanged(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.averageBrightness.HasValue)
        {
            m_Light.intensity = args.lightEstimation.averageBrightness.Value;
        }

        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            m_Light.colorTemperature = args.lightEstimation.averageColorTemperature.Value;
        }

        if (args.lightEstimation.colorCorrection.HasValue)
        {
            m_Light.color = args.lightEstimation.colorCorrection.Value;
        }
    }
}

