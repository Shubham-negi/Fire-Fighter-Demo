using UnityEngine;
using UnityEngine.Events;

public class QuizTapObject : MonoBehaviour
{
    [Header("Materials")]
    public Material correctMaterial;
    public Material wrongMaterial;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctVoice;
    public AudioClip wrongVoice;

    [Header("Events (Optional)")]
    public UnityEvent onCorrect;
    public UnityEvent onWrong;

    private bool isAnswered = false;

    // 👉 Call this on TAP (Button Down)
    public void OnTapCorrect()
    {
        if (isAnswered) return;

        isAnswered = true;

        ApplyMaterialToAllChildren(correctMaterial);
        PlayAudio(correctVoice);
        onCorrect?.Invoke();
    }

    public void OnTapWrong()
    {
        if (isAnswered) return;

        isAnswered = true;

        ApplyMaterialToAllChildren(wrongMaterial);
        PlayAudio(wrongVoice);
        onWrong?.Invoke();
    }

    void ApplyMaterialToAllChildren(Material mat)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            rend.material = mat;
        }
    }

    void PlayAudio(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    // Optional: Reset if needed
    public void ResetObject()
    {
        isAnswered = false;
    }
}