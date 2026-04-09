using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.Core.Utils;
using HurricaneVR.Framework.Shared;
using UnityEngine;

public class MonitorControllerHVR : MonoBehaviour
{
    // 🔹 Base (Y rotation)
    public Rigidbody monitorObj;
    public HVRRotationTracker handle1ValveTracker;
    public HVRRotationLimiter handle1Limiter;
    public float MaxYAngle = 360f;

    // 🔹 Head (X rotation)
    public Rigidbody monitorObjHead;
    public HVRRotationTracker handle2ValveTracker;
    public HVRRotationLimiter handle2Limiter;
    public float MaxXAngle = 45f;

    private Quaternion _startRotBase;
    private Quaternion _startRotHead;

    void Start()
    {
        _startRotBase = monitorObj.rotation;
        _startRotHead = monitorObjHead.rotation;
    }

    void FixedUpdate()
    {
        // ✅ Handle 1 → Y rotation (base)
        float yAngle = HVRUtilities.Remap(
            handle1ValveTracker.Angle,
            handle1Limiter.MinAngle,
            handle1Limiter.MaxAngle,
            0f,
            MaxYAngle
        );

        monitorObj.MoveRotation(
            _startRotBase * Quaternion.Euler(0f, yAngle, 0f)
        );

        // ✅ Handle 2 → X rotation (head)
        float xAngle = HVRUtilities.Remap(
            handle2ValveTracker.Angle,
            handle2Limiter.MinAngle,
            handle2Limiter.MaxAngle,
            0f,
            MaxXAngle
        );

        monitorObjHead.MoveRotation(
            _startRotHead * Quaternion.Euler(xAngle, 0f, 0f)
        );
    }
}