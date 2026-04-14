using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3AudioManager : MonoBehaviour
{
    public static Scene3AudioManager Instance;
    public AudioSource m_audioSource;
    // public AudioClip[] sounds;
    public AudioClip[] sounds;

  

    // ▶ Play sound by index
    public void PlaySound(int index)
    {
        m_audioSource.clip = sounds[index];

        m_audioSource.Play();
    }
    
    [Header("Voice Overs")]
    public AudioClip prepairPPEVO;

    public AudioClip followThePathToGetReadyVO;
    public AudioClip ppeKitIsReadyVO;
    public AudioClip startTrainingVO;
    public AudioClip climbUpToWaterCanonVO;
    public AudioClip climbDownFromWaterCanonVO;
    public AudioClip climbUpToFoamCanonVO;
    public AudioClip climbDownFromFoamCanonVO;
    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Ensure AudioSource
        m_audioSource = GetComponent<AudioSource>();
        if (m_audioSource == null)
            m_audioSource = gameObject.AddComponent<AudioSource>();
    }

    // =========================
    // 🔊 Base Play Method
    // =========================
    void Play(AudioClip clip)
    {
        if (clip == null) return;

        m_audioSource.Stop();          // Stop any current audio
        m_audioSource.clip = clip;     // Assign new clip
        m_audioSource.Play();          // Play
    }

    // =========================
    // 🎯 State Check (IMPORTANT)
    // =========================
    public bool IsPlaying()
    {
        return m_audioSource.isPlaying;
    }

    // =========================
    // ⏳ Play + Wait (Coroutine)
    // =========================
    public IEnumerator PlayAndWait(AudioClip clip)
    {
        if (clip == null) yield break;

        m_audioSource.Stop();
        m_audioSource.clip = clip;
        m_audioSource.Play();

        yield return new WaitWhile(() => m_audioSource.isPlaying);
    }

    // =========================
    // 🎤 Voice Overs
    // =========================
    public void PlayPrepairForPPEKITVO() => Play(prepairPPEVO);
    public void FollowThePathToGetReadyVO() => Play(followThePathToGetReadyVO);
    public void PPEKitIsReadyVO() => Play(ppeKitIsReadyVO);
}

