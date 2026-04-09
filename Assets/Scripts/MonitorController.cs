using UnityEngine;

public class MonitorController : MonoBehaviour
{
    [Header("Target Objects")]
    [SerializeField] private Transform objectRotateX;
    [SerializeField] private Transform objectRotateY;

    [Header("Rotation Settings")]
    [SerializeField] private float xMinAngle ;
    [SerializeField] private float xMaxAngle ;

    [SerializeField] private float yMinAngle ;
    [SerializeField] private float yMaxAngle ;

    // Store initial rotations so we don't destroy original orientation
    private Vector3 initialRotX;
    private Vector3 initialRotY;

    private void Start()
    {
        if (objectRotateX != null)
            initialRotX = objectRotateX.localEulerAngles;

        if (objectRotateY != null)
            initialRotY = objectRotateY.localEulerAngles;
    }

    // 🔹 Called by XR Knob 1 (for X rotation)
    public void RotateX(float knobValue)
    {
        if (objectRotateX == null) return;

        float angle = Mathf.Lerp(xMinAngle, xMaxAngle, knobValue);

        Vector3 rot = objectRotateX.localEulerAngles;
        rot.x = initialRotX.x + angle;

        objectRotateX.localEulerAngles = rot;
    }

    // 🔹 Called by XR Knob 2 (for Y rotation)
    public void RotateY(float knobValue)
    {
        if (objectRotateY == null) return;

        float angle = Mathf.Lerp(yMinAngle, yMaxAngle, knobValue);

        Vector3 rot = objectRotateY.localEulerAngles;
        rot.y = initialRotY.y + angle;

        objectRotateY.localEulerAngles = rot;
    }
}