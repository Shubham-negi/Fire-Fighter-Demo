using UnityEngine;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using UnityEngine.Events;

public class HVRSimpleInteractable : HVRGrabbable
{
    [Header("Events")]
    public UnityEvent OnTap;      // Called when pressed (grabbed)
    public UnityEvent OnRelease;  // Called when released

    // 👉 TAP (Button Down)
    protected override void OnGrabbed(HVRGrabberBase grabber)
    {
        base.OnGrabbed(grabber);

        OnTap?.Invoke();
    }

    // 👉 RELEASE (Button Up)
    protected override void OnReleased(HVRGrabberBase grabber)
    {
        base.OnReleased(grabber);

        OnRelease?.Invoke();
    }
}