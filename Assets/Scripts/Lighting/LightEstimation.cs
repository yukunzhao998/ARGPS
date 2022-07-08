using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class LightEstimation : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager arCameraManager;

    private Light currentLight;

    void Awake ()
    {
        currentLight = GetComponent<Light>();
    }

    private void OnEnable()
    {
        arCameraManager.frameReceived += FrameUpdated;
    }

    private void OnDisable()
    {
        arCameraManager.frameReceived -= FrameUpdated;
    }

    private void FrameUpdated(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.averageBrightness.HasValue)
        {
            currentLight.intensity = args.lightEstimation.averageBrightness.Value;
        }

//not supported by ARkit in the world tracking mode
/*
        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            currentLight.colorTemperature = args.lightEstimation.averageColorTemperature.Value;
        }

        if (args.lightEstimation.colorCorrection.HasValue)
        {
            currentLight.color = args.lightEstimation.colorCorrection.Value;
        }

        if (args.lightEstimation.mainLightDirection.HasValue)
        {
            mainLightDirectionValue.text = $"Direction: {args.lightEstimation.mainLightDirection.Value}";
            currentLight.transform.rotation = Quaternion.LookRotation(args.lightEstimation.mainLightDirection.Value);
        }

        if (args.lightEstimation.mainLightColor.HasValue)
        {
            mainLightColorValue.text = $"Main light color: {args.lightEstimation.mainLightColor.Value}";
            currentLight.color = args.lightEstimation.mainLightColor.Value;                   //here
        }

        if (args.lightEstimation.mainLightIntensityLumens.HasValue)
        {
            mainLightIntensityValue.text = $"Main light intensity: {args.lightEstimation.mainLightIntensityLumens.Value}";
            currentLight.intensity = args.lightEstimation.mainLightIntensityLumens.Value;
        }


        if (args.lightEstimation.ambientSphericalHarmonics.HasValue)
        {
            sphericalHarmonicsValue = args.lightEstimation.ambientSphericalHarmonics;
            RenderSettings.ambientMode = AmbientMode.Skybox;
            RenderSettings.ambientProbe = sphericalHarmonics.Value;
        }
*/

    }

}

