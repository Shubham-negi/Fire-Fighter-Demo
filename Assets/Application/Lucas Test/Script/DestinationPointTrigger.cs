using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DestinationPointTrigger : MonoBehaviour
{
    [Header("References")]
    public string playerTag = "Player";

    private void Reset()
    {
        // Make sure collider is a trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[DestinationPointTrigger] OnTriggerEnter on '{name}' by '{other.name}' (tag: {other.tag})");

        if (!other.CompareTag(playerTag))
        {
            Debug.Log($"[DestinationPointTrigger] Ignored � collider tag '{other.tag}' != playerTag '{playerTag}'");
            return;
        }

      

        // Tell subprocess to complete

        // Disable this destination object (arrow + trigger turn off)
        gameObject.SetActive(false);
    }
}
