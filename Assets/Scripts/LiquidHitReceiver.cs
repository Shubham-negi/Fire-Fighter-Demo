using UnityEngine;
using HighlightPlus;

public class LiquidHitReceiver : MonoBehaviour
{
    [Header("Cooling Settings (Water)")]
    public float coolingSpeed = 1f;
    public float hitInterval = 0.3f;

    [Range(0f, 1f)]
    public float coolingProgress = 0f;

    private bool isWaterHit = false;
    private bool isFoamHit = false;
    private bool cooledDownTriggered = false;

    private float hitTimer = 0f;

    [Header("Highlight")]
    public HighlightEffect highlight;

    [Header("Foam Settings")]
    public Material foamMaterial;
    public string gradientProperty = "_gradient";
    public float foamReduceSpeed = 0.01f;

    private float foamGradient = 1f;

    [Header("Fire Control")]
    public FireAndLightningManager fireAndLightningManager; // assign in inspector
    public float fireSlowFactor = 0.9f; // how much speed reduces each hit

    private Color hotColor = Color.red;
    private Color midColor = Color.green;
    private Color coolColor = Color.white;
    public GameObject waterSystem;
    public GameObject foamSystem;

    void Start()
    {
        if (highlight != null)
            highlight.highlighted = true;

        if (foamMaterial != null)
            foamGradient = foamMaterial.GetFloat(gradientProperty);
    }

    void Update()
    {
        hitTimer += Time.deltaTime;

        // =========================
        // 🔥 WATER COOLING
        // =========================
        if (isWaterHit && hitTimer >= hitInterval)
        {
            hitTimer = 0f;

            float resistance = 1f - coolingProgress;
            float coolingStep = coolingSpeed * resistance * 0.02f;

            coolingProgress += coolingStep;
        }

        coolingProgress = Mathf.Clamp01(coolingProgress);
        UpdateHighlight();

        // =========================
        // 🧯 FOAM EFFECT
        // =========================
        if (isFoamHit)
        {
            ApplyFoamEffect();
        }

        // Reset flags
        isWaterHit = false;
        isFoamHit = false;

        // Final cooling state
        if (coolingProgress >= 0.98f && !cooledDownTriggered)
        {
            cooledDownTriggered = true;
            CooledDown();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.name == "Water Stream")
        {
            isWaterHit = true;
        }
        else if (other.name == "Foam Stream")
        {
            isFoamHit = true;
        }
    }

    // =========================
    // 🔥 Highlight Cooling
    // =========================
    void UpdateHighlight()
    {
        if (highlight == null) return;

        Color targetColor;

        if (coolingProgress < 0.5f)
            targetColor = Color.Lerp(hotColor, midColor, coolingProgress * 2f);
        else
            targetColor = Color.Lerp(midColor, coolColor, (coolingProgress - 0.5f) * 2f);

        highlight.overlayColor = Color.Lerp(highlight.overlayColor, targetColor, Time.deltaTime * 2f);
        highlight.innerGlowColor = Color.Lerp(highlight.innerGlowColor, targetColor, Time.deltaTime * 2f);
    }

    // =========================
    // 🧯 Foam Logic
    // =========================
    void ApplyFoamEffect()
    {
        // 🔹 Reduce foam gradient
        if (foamMaterial != null)
        {
            foamGradient -= foamReduceSpeed * Time.deltaTime;
            foamGradient = Mathf.Clamp01(foamGradient);

            foamMaterial.SetFloat(gradientProperty, foamGradient);
        }

        if (fireAndLightningManager == null)
            return;

        ParticleSystem[] fireParticles =
            fireAndLightningManager.fireParticlesObject.GetComponentsInChildren<ParticleSystem>(true);

        bool allFiresOff = true; // 🔥 TRACK GLOBAL STATE

        foreach (ParticleSystem ps in fireParticles)
        {
            if (ps == null)
                continue;

            var emission = ps.emission;

            float currentRate = emission.rateOverTime.constant;

            // Reduce gradually
            float newRate = Mathf.Lerp(currentRate, 0f, Time.deltaTime * 1.5f);

            emission.rateOverTime = newRate;

            // 🔥 Check if still active
            if (newRate > 1f)
            {
                allFiresOff = false;
            }
            else
            {
                // Stop completely
                emission.rateOverTime = 0f;

                if (ps.isPlaying)
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);

                ps.gameObject.SetActive(false);
            }
        }

        // ✅ AFTER LOOP → CHECK EVERYTHING
        if (allFiresOff)
        {
            fireAndLightningManager.fireParticlesObject.SetActive(false);

            // (Optional) Reset state machine
            // fireAndLightningManager.ResetToIdle();
        }
    }

    // =========================
    // ✅ Final State
    // =========================
    void CooledDown()
    {
        Debug.Log("✅ Object cooled down!");

        if (highlight != null)
        {
            highlight.SetHighlighted(false);
            Invoke(nameof(DisableHighlight), 0.5f);
        }
    }

    void DisableHighlight()
    {
        if (highlight != null)
            highlight.enabled = false;
    }


    /// <summary>
    /// WATER SYSTEM CONTROL 
    /// </summary>

    public void waterStepCounter(int step)
    {
        if (step == 2)
        {
            waterSystem.SetActive(true);
        }
        else
        {
            waterSystem.SetActive(false);
        }
    }

    public void FoamStepCounter(int step)
    {
        if (step == 2)
        {
            foamSystem.SetActive(true);
        }
        else
        {
            foamSystem.SetActive(false);
        }
    }
void ResetFoam()
{
    foamGradient = 1f;

    if (foamMaterial != null)
    {
        foamMaterial.SetFloat(gradientProperty, foamGradient);
    }
}

void OnApplicationQuit()
{
    ResetFoam();
}

void OnDisable()
{
    ResetFoam();
}


    /// <summary>
    /// WATER SYSTEM CONTROL 
    /// </summary>
    /// 
    /// 
    /// 


}