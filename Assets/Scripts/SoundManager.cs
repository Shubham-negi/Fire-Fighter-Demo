using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;
    public AudioClip[] sounds; // assign in inspector

    void Awake()
    {
        Instance = this;
    }

    // ▶ Play sound by index
    public void PlaySound(int index)
    {
        if (index < 0 || index >= sounds.Length)
        {
            Debug.LogWarning("Sound index out of range");
            return;
        }

        audioSource.PlayOneShot(sounds[index]);
    }

    // ▶ Play loop (for background/fire)
    public void PlayLoop(int index)
    {
        if (index < 0 || index >= sounds.Length) return;

        audioSource.clip = sounds[index];
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopLoop()
    {
        audioSource.Stop();
    }
}