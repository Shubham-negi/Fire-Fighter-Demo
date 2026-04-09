using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.Core.Utils;
using UnityEngine;

public class MonitorRotateX : MonoBehaviour
{
    public Transform monitorObjHead; // 🔥 use Transform, not Rigidbody

    public HVRRotationTracker handle2ValveTracker;
    public HVRRotationLimiter handle2Limiter;

    public float MaxXAngle = 45f;

    void Update() // 🔥 use Update, not FixedUpdate
    {
        float xAngle = HVRUtilities.Remap(
            handle2ValveTracker.Angle,
            handle2Limiter.MinAngle,
            handle2Limiter.MaxAngle,
            0f,
            MaxXAngle
        );

        // ✅ Local rotation → respects parent movement
        monitorObjHead.localRotation = Quaternion.Euler(xAngle, 0f, 0f);
    }
}