using UnityEngine;

public class FireExtinguisherHandle : MonoBehaviour
{
    [Range(0, 1)]
    public float knob;

    public Transform handle;          // rotating handle
    public float maxAngle = 30f;      // squeeze angle

    public ParticleSystem spray;
    public AudioSource sprayAudio;

    bool isSpraying;

    void Update()
    {
        float angle = knob * maxAngle;
        handle.localRotation = Quaternion.Euler(0, 0, angle);

        // start spray
        if (knob > 0.8f && !isSpraying)
        {
            StartSpray();
        }

        // stop spray
        if (knob < 0.2f && isSpraying)
        {
            StopSpray();
        }
    }

    void StartSpray()
    {
        isSpraying = true;
        spray.Play();
        sprayAudio.Play();
    }

    void StopSpray()
    {
        isSpraying = false;
        spray.Stop();
        sprayAudio.Stop();
    }
    public void SetKnobActive(float value)
    {
        knob = Mathf.Clamp01(value);
    }

     public void SetKnobDeactive(float value)
    {
        knob = Mathf.Clamp01(value);
    }
}