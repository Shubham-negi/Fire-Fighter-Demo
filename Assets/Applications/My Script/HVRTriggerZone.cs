using UnityEngine;
using UnityEngine.Events;

public class HVRTriggerZone : MonoBehaviour
{
    [Header("Settings")]
    public string playerTag = "VRPlayer";
    public bool triggerOnce = true;

    [Header("Events")]
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered by: " + other.name);

        // ✅ Check root object (works for hands, body, etc.)
        if (other.transform.root.CompareTag(playerTag))
        {
            if (triggerOnce && hasTriggered) return;

            hasTriggered = true;

            Debug.Log("✅ Player Entered Trigger");
            OnPlayerEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited by: " + other.name);

        if (other.transform.root.CompareTag(playerTag))
        {
            Debug.Log("❌ Player Exited Trigger");
            OnPlayerExit?.Invoke();
        }
    }

    // Optional: Reset trigger (useful for replay)
    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}