using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidHitReceiver : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Hit by: " + other.name);

        // Example reaction
        ActivateEffect();
    }

    void ActivateEffect()
    {
        Debug.Log("🔥 Object reacting to water!");
    }
}
