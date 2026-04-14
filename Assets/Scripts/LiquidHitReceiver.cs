using UnityEngine;
using HighlightPlus;
using UnityEngine.Events;

public class LiquidHitReceiver : MonoBehaviour
{

   public UnityEvent onFireCooledDown;

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
    public FireAndLightningManager fireAndLightningManager;

    // 🔥 NEW — CONTROL FIRE SPEED HERE
    public float reductionPerSecond = 30f; // 🔥 LOWER = SLOWER FIRE

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

        isWaterHit = false;
        isFoamHit = false;

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
    // 🧯 Foam Logic (FIXED)
    // =========================
    void ApplyFoamEffect()
    {
        // 🔹 Foam visual reduce
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

        bool allFiresOff = true;

        foreach (ParticleSystem ps in fireParticles)
        {
            if (ps == null)
                continue;

            var emission = ps.emission;
            float currentRate = emission.rateOverTime.constant;

            // 🔥 NEW — SLOW LINEAR REDUCTION
            // 🔥 Smooth slow reduction
            float newRate = Mathf.Lerp(currentRate, 0f, Time.deltaTime * 0.3f);
            newRate = Mathf.Max(newRate, 0f);

            emission.rateOverTime = newRate;

            // 🔥 DO NOT cut suddenly — let it fade naturally
            if (newRate > 0.1f)
            {
                allFiresOff = false;
            }
            else
            {
                // Stop emission ONLY (no sudden disable)
                emission.rateOverTime = 0f;

                if (ps.isPlaying)
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);

                // ❌ IMPORTANT: REMOVE THIS LINE (if exists)
                // ps.gameObject.SetActive(false);
            }
        }

        if (allFiresOff)
        {
            fireAndLightningManager.fireParticlesObject.SetActive(false);
            foamSystem.SetActive(false);
            onFireCooledDown.Invoke();
            
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

    // =========================
    // 💧 WATER CONTROL
    // =========================
    public void waterStepCounter(int step)
    {
        waterSystem.SetActive(step == 2);
    }

    public void FoamStepCounter(int step)
    {
        foamSystem.SetActive(step == 2);
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
}