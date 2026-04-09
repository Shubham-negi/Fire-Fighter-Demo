using UnityEngine;

[RequireComponent(typeof(Collider))]
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
    private ParticleSystem.MainModule main;

    private Collider fireCollider;

    void Start()
    {
        // 🔹 Auto-fetch components (works even if on child)
        fireParticles = GetComponentInChildren<ParticleSystem>();
        fireCollider = GetComponentInChildren<Collider>();

        // 🔹 Safety checks
        if (fireParticles == null)
        {
            Debug.LogError($"❌ No ParticleSystem found in {gameObject.name}");
            return;
        }

        if (fireCollider == null)
        {
            Debug.LogWarning($"⚠️ No Collider found in {gameObject.name}");
        }

        // 🔹 Cache modules (performance)
        emission = fireParticles.emission;
        main = fireParticles.main;

        currentIntensity = maxIntensity;
    }

    void Update()
    {
        if (isExtinguished) return;

        // 🔹 Fire intensity logic
        if (isBeingSprayed)
        {
            currentIntensity -= extinguishRate * Time.deltaTime;
        }
        else
        {
            currentIntensity += recoverRate * Time.deltaTime;
        }

        currentIntensity = Mathf.Clamp(currentIntensity, 0, maxIntensity);

        UpdateFireVisual();

        // 🔹 Fully extinguished
        if (currentIntensity <= 0)
        {
            ExtinguishCompletely();
        }

        // 🔹 Reset spray flag each frame
        isBeingSprayed = false;
    }

    // 🔹 Called externally (from water/foam system)
    public void SprayWater()
    {
        if (!isExtinguished)
        {
            isBeingSprayed = true;
        }
    }

    void UpdateFireVisual()
    {
        float normalized = currentIntensity / maxIntensity;

        // 🔹 Control emission
        emission.rateOverTime = 100f * normalized;

        // 🔹 Smooth size change
        float targetSize = 1.5f * normalized;
        main.startSize = Mathf.Lerp(main.startSize.constant, targetSize, Time.deltaTime * 5f);
    }

    void ExtinguishCompletely()
    {
        isExtinguished = true;

        // 🔹 Stop particles
        fireParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // 🔹 Disable collider so it doesn't block other fires
        if (fireCollider != null)
        {
            fireCollider.enabled = false;
        }

        // 🔹 Optional: move to different layer (advanced control)
        // gameObject.layer = LayerMask.NameToLayer("ExtinguishedFire");
    }
}