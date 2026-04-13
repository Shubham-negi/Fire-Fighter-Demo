using Unity.VisualScripting;
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
    }

    // =========================
    // 🔹 SPRINKLER SCALE (INVERSE)
    // =========================
    void UpdateSprinkler()
    {
        if (!sprinkler) return;

        if (flowRate <= offThreshold)
        {
            sprinkler.localScale = Vector3.zero;
            return;
        }

        // 🔹 Peak behavior (0 → 0.5 → 1 → 0)
        float peak = 1f - Mathf.Abs(flowRate * 2f - 1f);

        // 🔹 Apply scale using peak
        float scaleValue = Mathf.Lerp(0f, maxZScale, peak);

        sprinkler.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

        // 🔹 Rotation still linear (or you can also peak it if needed)
        float rotation = Mathf.Lerp(minRotation, maxRotation, flowRate);
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
   

    // =========================
    // 🔻 TURN OFF SYSTEM
    // =========================
    void TurnOffSystem()
    {
        if (waterStream && waterStream.isPlaying)
            waterStream.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

      

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

      
    }




 public void AngleChangedHandler(float angle, float delta)
{
    print("Angle changed: " + angle + ", Delta: " + delta);

    // 🔹 Convert 360-like values back to -180 → 180
    if (angle > 180f)
        angle -= 360f;

    // 🔹 Clamp between 0 → 25
    float normalized = Mathf.Clamp(angle, 0f, 25f);

    // 🔹 Reverse mapping
    flowRate = 1f - (normalized / 25f);
}
}