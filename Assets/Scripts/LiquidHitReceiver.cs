using UnityEngine;
using HighlightPlus;

public class LiquidHitReceiver : MonoBehaviour
{
    [Header("Cooling Settings")]
    public float coolingSpeed = 1f;          // base cooling strength (keep low)
    public float hitInterval = 0.3f;         // delay between cooling steps (VERY IMPORTANT)

    [Range(0f, 1f)]
    public float coolingProgress = 0f;

    private bool isBeingHit = false;
    private bool cooledDownTriggered = false;

    private float hitTimer = 0f;

    [Header("Highlight")]
    public HighlightEffect highlight;

    private Color hotColor = Color.red;
    private Color midColor = Color.green;
    private Color coolColor = Color.white;

    void Start()
    {
        if (highlight != null)
        {
            highlight.highlighted = true;
        }
    }

    void Update()
    {
        hitTimer += Time.deltaTime;

        // 🔹 Apply cooling only at intervals (prevents particle spam speed)
        if (isBeingHit && hitTimer >= hitInterval)
        {
            hitTimer = 0f;

            // Resistance makes cooling slower near the end
            float resistance = 1f - coolingProgress;

            // VERY SMALL step → super slow cooling
            float coolingStep = coolingSpeed * resistance * 0.02f;

            coolingProgress += coolingStep;
        }

        coolingProgress = Mathf.Clamp01(coolingProgress);

        UpdateHighlight();

        // Reset hit flag every frame
        isBeingHit = false;

        // 🔹 Final state
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
            isBeingHit = true;
        }
    }

    void UpdateHighlight()
    {
        if (highlight == null) return;

        Color targetColor;

        if (coolingProgress < 0.5f)
        {
            // Red → Green
            targetColor = Color.Lerp(hotColor, midColor, coolingProgress * 2f);
        }
        else
        {
            // Green → White
            targetColor = Color.Lerp(midColor, coolColor, (coolingProgress - 0.5f) * 2f);
        }

        // 🔹 Smooth transition (important for visual clarity)
        highlight.overlayColor = Color.Lerp(highlight.overlayColor, targetColor, Time.deltaTime * 2f);
        highlight.innerGlowColor = Color.Lerp(highlight.innerGlowColor, targetColor, Time.deltaTime * 2f);
    }

    void CooledDown()
    {
        Debug.Log("✅ Object cooled down!");

        if (highlight != null)
        {
            highlight.SetHighlighted(false);

            // Disable component after short delay
            Invoke(nameof(DisableHighlight), 0.5f);
        }
    }

    void DisableHighlight()
    {
        if (highlight != null)
        {
            highlight.enabled = false;
        }
    }
}