using UnityEngine;
using UnityEngine.Events;

public class ParticleHitVoiceTag : MonoBehaviour
{
    public string targetTag = "FlameWood";

    public UnityEvent onParticleHit;

    bool hasPlayed;

    public void Start()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag(targetTag) && !hasPlayed)
        {
            onParticleHit.Invoke();
            hasPlayed = true;
        }
    }  
}