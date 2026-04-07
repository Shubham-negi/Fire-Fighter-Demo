using UnityEngine;

public class FireController : MonoBehaviour
{
    public ParticleSystem fireParticles;

    public float maxIntensity = 100f;
    private float currentIntensity;

    public float extinguishRate = 30f;
    public float recoverRate = 10f;

    private bool isBeingSprayed = false;
    private bool isExtinguished = false;

    private ParticleSystem.EmissionModule emission;

    void Start()
    {
        if (fireParticles == null)
        {
            fireParticles = GetComponent<ParticleSystem>();
            Debug.LogWarning($"[{name}] FireParticles was not assigned, using local ParticleSystem.");
        }

        if (fireParticles == null)
        {
            Debug.LogError($"[{name}] ❌ No ParticleSystem found!");
            return;
        }

        currentIntensity = maxIntensity;
        emission = fireParticles.emission;

        Debug.Log($"[{name}] 🔥 Fire Initialized with intensity: {currentIntensity}");
    }

    void Update()
    {
        if (isExtinguished) return;

        if (isBeingSprayed)
        {
            currentIntensity -= extinguishRate * Time.deltaTime;
            Debug.Log($"[{name}] 💧 Being sprayed. Intensity: {currentIntensity:F2}");
        }
        else
        {
            currentIntensity += recoverRate * Time.deltaTime;
        }

        currentIntensity = Mathf.Clamp(currentIntensity, 0, maxIntensity);

        UpdateFireVisual();

        if (currentIntensity <= 0)
        {
            ExtinguishCompletely();
        }

        isBeingSprayed = false;
    }

    public void SprayWater()
    {
        if (!isExtinguished)
        {
            isBeingSprayed = true;
            Debug.Log($"[{name}] 💦 Water hit detected!");
        }
    }

    void UpdateFireVisual()
    {
        float normalized = currentIntensity / maxIntensity;

        var rate = emission.rateOverTime;
        rate.constant = 100f * normalized;
        emission.rateOverTime = rate;

        var main = fireParticles.main;
        main.startSize = Mathf.Lerp(main.startSize.constant, 1.5f * normalized, Time.deltaTime * 5f);
    }

    void ExtinguishCompletely()
    {
        isExtinguished = true;

        Debug.Log($"[{name}] ✅ FIRE EXTINGUISHED!");

        fireParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}