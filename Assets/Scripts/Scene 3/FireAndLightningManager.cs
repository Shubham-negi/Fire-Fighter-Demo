using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAndLightningManager : MonoBehaviour
{
    public enum FireState
    {
        Idle,
        Lightning,
        FireSequence
    }

    [Header("Lightning")]
    public GameObject lightningObject;

    [Header("Fire Particles object")]
    public GameObject fireParticlesObject;

    private List<ParticleSystem> fireParticles = new List<ParticleSystem>();

    [Header("Timing")]
    public float delayAfterLightning = 3f;
    public float delayBetweenEachFire = 0.5f;

    [Header("Emission Settings")]
    public float emissionTarget = 50f;
    public float emissionRampSpeed = 20f;

    private FireState currentState = FireState.Idle;

    void Awake()
    {
        CollectFireParticles();
    }

    // 🔹 Collect all child particle systems
    void CollectFireParticles()
    {
        fireParticles.Clear();

        if (fireParticlesObject == null)
        {
            Debug.LogWarning("Fire Parent not assigned!");
            return;
        }

        var particles = fireParticlesObject.GetComponentsInChildren<ParticleSystem>(true);

        foreach (var ps in particles)
        {
            fireParticles.Add(ps);
        }
    }

    // 🔹 PUBLIC ENTRY POINT
    [ContextMenu("Start Fire")]
    public void StartFire()
    {
        if (currentState != FireState.Idle)
            return;

        StartCoroutine(FireFlow());
    }

    private IEnumerator FireFlow()
    {
        // 🔥 STATE 1: LIGHTNING
        currentState = FireState.Lightning;

        if (lightningObject != null)
            lightningObject.SetActive(true);

        yield return new WaitForSeconds(delayAfterLightning);
fireParticlesObject.SetActive(true);
        // 🔥 STATE 2: FIRE SEQUENCE
        currentState = FireState.FireSequence;

        for (int i = 0; i < fireParticles.Count; i++)
        {
            StartCoroutine(StartFireParticle(fireParticles[i]));
            yield return new WaitForSeconds(delayBetweenEachFire);
        }
    }

    private IEnumerator StartFireParticle(ParticleSystem ps)
    {
        if (ps == null)
            yield break;
            lightningObject.SetActive(false);

        var emission = ps.emission;

        // Start from 0
        emission.rateOverTime = 0;

        ps.gameObject.SetActive(true);
        ps.Play();

        float currentRate = 0;

        // Smooth ramp
        while (currentRate < emissionTarget)
        {
            currentRate += emissionRampSpeed * Time.deltaTime;

            var rate = emission.rateOverTime;
            rate.constant = currentRate;
            emission.rateOverTime = rate;

            yield return null;
        }

        // Clamp final value
        var finalRate = emission.rateOverTime;
        finalRate.constant = emissionTarget;
        emission.rateOverTime = finalRate;
    }
}