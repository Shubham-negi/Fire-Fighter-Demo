using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class FireController : MonoBehaviour
{
    [System.Serializable]
    public class FireElement
    {
        public ParticleSystem fireParticle;
        public AudioSource fireAudio;
        public Light fireLight;

        [HideInInspector] public ParticleSystem.MainModule mainModule;
        [HideInInspector] public ParticleSystem.EmissionModule emissionModule;
        [HideInInspector] public float currentEmission;

        [Header("Individual Settings")]
        public float maxEmission = 80f;
        public float minEmission = 0f;
    }

    [Header("All Fire Elements")]
    public List<FireElement> fires = new List<FireElement>();

    [Header("Global Settings")]
    public float extinguishSpeed = 5f;
    public float extinguishThreshold = 2f;

    [Header("Visual Settings")]
    public float maxStartSize = 1.2f;
    public float minStartSize = 0.1f;
    public float maxLifetime = 2f;
    public float minLifetime = 0.2f;

    [Header("Debug")]
    public bool debugEnabled = false;
    public string extinguisherTag = "ExtinguisherGas";

    private bool isExtinguishing = false;
    private bool isFullyExtinguished = false;

    void Start()
    {
        foreach (var fire in fires)
        {
            if (fire.fireParticle == null) continue;

            fire.mainModule = fire.fireParticle.main;
            fire.emissionModule = fire.fireParticle.emission;

            fire.currentEmission = fire.maxEmission;
            fire.emissionModule.rateOverTime = fire.currentEmission;

            if (!fire.fireParticle.isPlaying)
                fire.fireParticle.Play();

            if (fire.fireAudio)
            {
                fire.fireAudio.loop = true;
                fire.fireAudio.volume = 1f;
                fire.fireAudio.Play();
            }

            if (fire.fireLight)
                fire.fireLight.intensity = 2f;
        }
    }

    void Update()
    {
        if (isFullyExtinguished)
            return;

        foreach (var fire in fires)
        {
            if (fire.fireParticle == null) continue;

            if (isExtinguishing)
            {
                // 🔥 Reduce fire
                fire.currentEmission -= extinguishSpeed * Time.deltaTime;
                fire.currentEmission = Mathf.Max(fire.currentEmission, fire.minEmission);
            }

            fire.emissionModule.rateOverTime = fire.currentEmission;

            float t = fire.currentEmission / fire.maxEmission;

            fire.mainModule.startSize = Mathf.Lerp(minStartSize, maxStartSize, t);
            fire.mainModule.startLifetime = Mathf.Lerp(minLifetime, maxLifetime, t);

            if (fire.fireAudio)
                fire.fireAudio.volume = Mathf.Lerp(0f, 1f, t);

            if (fire.fireLight)
                fire.fireLight.intensity = Mathf.Lerp(0f, 2f, t);

            // 🔴 FULLY EXTINGUISHED
            if (fire.currentEmission <= extinguishThreshold)
            {
                fire.currentEmission = 0;
                fire.emissionModule.rateOverTime = 0;

                // Stop effects
                fire.fireParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

                if (fire.fireAudio) fire.fireAudio.Stop();
                if (fire.fireLight) fire.fireLight.intensity = 0;

                // 🔴 Disable GameObject
                fire.fireParticle.gameObject.SetActive(false);
            }
        }

        // ✅ Check if ALL fires are extinguished
        bool allOff = true;
        foreach (var fire in fires)
        {
            if (fire.currentEmission > extinguishThreshold)
            {
                allOff = false;
                break;
            }
        }

        if (allOff)
        {
            isFullyExtinguished = true;

            if (debugEnabled)
                Debug.Log("🔥 All fires permanently extinguished!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(extinguisherTag))
        {
            isExtinguishing = true;

            if (debugEnabled)
                Debug.Log("🧯 Extinguishing Started");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(extinguisherTag))
            isExtinguishing = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(extinguisherTag))
        {
            isExtinguishing = false;

            if (debugEnabled)
                Debug.Log("🧯 Extinguishing Stopped");
        }
    }
}