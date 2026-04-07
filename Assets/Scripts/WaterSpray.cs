using UnityEngine;
using System.Collections.Generic;

public class WaterSpray : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        if (ps == null)
        {
            Debug.LogError($"[{name}] ❌ No ParticleSystem found!");
        }
        else
        {
            Debug.Log($"[{name}] 💧 WaterSpray initialized.");
        }
    }

    // 🔹 Called when particles hit something
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log($"[{name}] 💥 Particle collided with: {other.name}");

        FireController fire = other.GetComponent<FireController>();

        if (fire != null)
        {
            Debug.Log($"[{name}] 🔥 Fire detected → Applying water");
            fire.SprayWater();
        }
        else
        {
            Debug.Log($"[{name}] ⚠ Hit object has NO FireController");
        }
    }
}