using System.Collections.Generic;
using UnityEngine;

public class FireFighterSoundManager : MonoBehaviour
{

    public AudioSource m_audioSource;     
    // public AudioClip[] sounds;
    public AudioClip[] sounds;

    void Awake()
    {
    }

    // ▶ Play sound by index
    public void PlaySound(int index)
    {
        Debug.Log("Playing sound: at index = " +index);
        Debug.Log("Playing sound: at  sounds Length = " +sounds.Length);
       m_audioSource.clip = sounds[index];

        m_audioSource.Play();
    }
}

