using HighlightPlus;
using UnityEngine;

public class FireAlarmButton : MonoBehaviour
{
    public GameObject OrignalGlass;
    public GameObject brokernGlass;
    public HighlightEffect highlightEffect;
    public AudioSource audioSource;
    public AudioClip audioClip;

    private bool triggered;

    private void OnCollisionEnter(Collision collision)
    {
        if (triggered) return;

        // optional: only hand hit
        // if (!collision.collider.CompareTag("Hand")) return;

        triggered = true;

        OrignalGlass.SetActive(false);
        brokernGlass.SetActive(true);

        if (audioSource && audioClip)
            audioSource.PlayOneShot(audioClip);

        if (highlightEffect)
            highlightEffect.enabled = false;
    }
}