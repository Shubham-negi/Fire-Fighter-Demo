using UnityEngine;

public class LiquidFlowController : MonoBehaviour
{
    [Header("Flow Control")]
    [Range(0f, 1f)]
    public float flowRate = 0f;

    [Header("Cutoff")]
    public float offThreshold = 0.01f;

    // =========================
    // 🔹 SPRINKLER
    // =========================
    [Header("Sprinkler")]
    public Transform sprinkler;
    public float minZScale = 0.2f;  // high flow
    public float maxZScale = 1f;    // low flow
    public float minRotation = -50f;
public float maxRotation = 0f;

    // =========================
    // 🔹 WATER STREAM
    // =========================
    [Header("Water Stream")]
    public ParticleSystem waterStream;
    public float minStreamSpeed = 1f;
    public float maxStreamSpeed = 20f;

    public float minStreamScale = 0.2f;
public float maxStreamScale = 1.5f;

    // =========================
    // 🔹 DROPLETS
    // =========================
    [Header("Droplets")]
    public ParticleSystem droplets;

    public float minDropletSpeed = 1f;
    public float maxDropletSpeed = 20f;

    public float minDropletSize = 0.1f; // high flow
    public float maxDropletSize = 0.1f; // low flow (you kept same — adjustable)

    public float minSpread = 10f;  // high flow
    public float maxSpread = 25f;  // low flow

    public float minEmission = 100f; // high flow
    public float maxEmission = 300f; // low flow

    void Update()
    {
        // 🔻 FULL SHUTDOWN
        if (flowRate <= offThreshold)
        {
            TurnOffSystem();
            return;
        }

        // 🔺 ENSURE SYSTEM IS RUNNING
        TurnOnSystem();

        UpdateSprinkler();
        UpdateWaterStream();
        UpdateDroplets();
    }

    // =========================
    // 🔹 SPRINKLER SCALE (INVERSE)
    // =========================
    void UpdateSprinkler()
{
    if (!sprinkler) return;

    // 🔹 If completely off → hide sprinkler
    if (flowRate <= offThreshold)
    {
        sprinkler.localScale = Vector3.zero;
        return;
    }

    // 🔹 Inverse scaling (flow ↑ → scale ↓)
    float scaleValue = Mathf.Lerp(maxZScale, minZScale, flowRate);

    sprinkler.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

    // 🔹 Rotation (flow ↑ → rotate from min → max)
    float rotation = Mathf.Lerp(minRotation, maxRotation, flowRate);

    // 👉 Rotate on X axis (change axis if needed)
    sprinkler.localRotation = Quaternion.Euler(rotation, 0f, 0f);
}

    // =========================
    // 🔹 WATER STREAM (DIRECT)
    // =========================
    void UpdateWaterStream()
{
    if (!waterStream) return;
 var main = waterStream.main;
        

        // 🔹 Speed (direct)
        float speed = Mathf.Lerp(minDropletSpeed, maxDropletSpeed, flowRate);
        main.startSpeed = speed;
    // 🔹 Scale increases with flow
    float scaleValue = Mathf.Lerp(minStreamScale, maxStreamScale, flowRate);

    // 👉 Usually stream stretches in Z (forward axis)
    Vector3 scale = waterStream.transform.localScale;
    scale.z = scaleValue;

    // Optional: also widen stream slightly
    scale.x = scaleValue * 0.5f;
    scale.y = scaleValue * 0.5f;

    waterStream.transform.localScale = scale;
}

    // =========================
    // 🔹 DROPLETS (ADVANCED BEHAVIOR)
    // =========================
    void UpdateDroplets()
    {
        if (!droplets) return;

        var main = droplets.main;
        var emission = droplets.emission;
        var shape = droplets.shape;

        // 🔹 Speed (direct)
        float speed = Mathf.Lerp(minDropletSpeed, maxDropletSpeed, flowRate);
        main.startSpeed = speed;

        // 🔹 Inverse behavior
        float inverse = 1f - flowRate;

        float size = Mathf.Lerp(minDropletSize, maxDropletSize, inverse);
        float spread = Mathf.Lerp(minSpread, maxSpread, inverse);
        float rate = Mathf.Lerp(minEmission, maxEmission, inverse);

        // ✅ Size (X/Y)
        main.startSizeX = size;
        main.startSizeY = size;

        // ✅ Spread (depends on particle shape)
        shape.angle = spread;                 // for cone
        shape.radius = spread * 0.01f;        // for sphere/circle

        // ✅ Emission
        var rateOverTime = emission.rateOverTime;
        rateOverTime.constant = rate;
        emission.rateOverTime = rateOverTime;
    }

    // =========================
    // 🔻 TURN OFF SYSTEM
    // =========================
    void TurnOffSystem()
    {
        if (waterStream && waterStream.isPlaying)
            waterStream.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (droplets && droplets.isPlaying)
            droplets.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // Reset sprinkler (closed state)
        // 🔹 Fully hide sprinkler when off
        if (sprinkler)
        {
            sprinkler.localScale = Vector3.zero;
        }
    }

    // =========================
    // 🔺 TURN ON SYSTEM
    // =========================
    void TurnOnSystem()
    {
        if (waterStream && !waterStream.isPlaying)
            waterStream.Play();

        if (droplets && !droplets.isPlaying)
            droplets.Play();
    }
}