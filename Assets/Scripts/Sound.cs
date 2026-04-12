using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource m_audioSource;
    public AudioClip m_audioClip;

    void Start()
    {
        m_audioSource.clip = m_audioClip;
        m_audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
