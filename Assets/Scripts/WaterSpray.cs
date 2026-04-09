using UnityEngine;
using System.Collections.Generic;

public class WaterSpray : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

       
    }

    // 🔹 Called when particles hit something
    private void OnParticleCollision(GameObject other)
    {

        FireController fire = other.GetComponent<FireController>();

        if (fire != null)
        {
            print($"💧 Hit fire: {other.name}");
            fire.SprayWater();
        }
        
    }
}