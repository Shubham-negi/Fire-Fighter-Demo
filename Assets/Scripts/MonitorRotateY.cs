using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.Core.Utils;
using UnityEngine;

public class MonitorRotateY : MonoBehaviour
{
    public Transform monitorObj;

    public HVRRotationTracker handle1ValveTracker;
    public HVRRotationLimiter handle1Limiter;

    public float MaxYAngle = 360f;

    private Quaternion _startRotation;

    void Start()
    {
        _startRotation = monitorObj.rotation;
    }

    void FixedUpdate()
    {
        float yAngle = HVRUtilities.Remap(
            handle1ValveTracker.Angle,
            handle1Limiter.MinAngle,
            handle1Limiter.MaxAngle,
            0f,
            MaxYAngle
        );

               monitorObj.localRotation = Quaternion.Euler(0f, yAngle, 0f);

    }
}