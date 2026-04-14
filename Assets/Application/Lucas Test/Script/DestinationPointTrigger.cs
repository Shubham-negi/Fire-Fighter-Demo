using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class DestinationPointTrigger : MonoBehaviour
{
    [Header("References")]
    public string playerTag = "Player";

    [Header("Events")]
    public UnityEvent onReachedDestination; // 🔹 assign in Inspector

    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[DestinationPointTrigger] OnTriggerEnter on '{name}' by '{other.name}' (tag: {other.tag})");

        if (!other.CompareTag(playerTag))
        {
            Debug.Log($"Ignored collider tag '{other.tag}' != playerTag '{playerTag}'");
            return;
        }

        // 🔹 Invoke event
        onReachedDestination?.Invoke();

        // 🔹 Disable object
        gameObject.SetActive(false);
    }
}