using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class LightEstimation : MonoBehaviour
{
    [SerializeField] ARCameraManager cameraManager;
    [SerializeField] Light lightObj;

    private void OnEnable()
    {
        cameraManager.frameReceived += FrameUpdated;
    }

    private void OnDisable()
    {
        cameraManager.frameReceived -= FrameUpdated;
    }

    private void FrameUpdated(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.averageBrightness.HasValue)
        {
            lightObj.intensity = args.lightEstimation.averageBrightness.Value;
        }

        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            lightObj.colorTemperature = args.lightEstimation.averageColorTemperature.Value;
        }

        if (args.lightEstimation.colorCorrection.HasValue)
        {
            lightObj.color = args.lightEstimation.colorCorrection.Value;
        }
    }
}
