//using UnityEngine;
//using UnityEngine.Events;

//public class ParticleHitVoiceTag : MonoBehaviour
//{
//    public string targetTag = "FlameWood";

//    public UnityEvent onParticleHit;

//    bool hasPlayed;

//    public void Start()
//    {

//    }

//    private void OnParticleCollision(GameObject other)
//    {
//        if (other.CompareTag(targetTag) && !hasPlayed)
//        {
//            Debug.Log("Hit GameObject Name : " + other.gameObject.name);
//            onParticleHit.Invoke();
//            hasPlayed = true;
//        }
//    }  


//}




using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ParticleHitVoiceTag : MonoBehaviour
{
    public string targetTag = "FlameWood";

    public UnityEvent onParticleHit;

    [Header("Scale Settings")]
    public float scaleDuration = 1.5f; // time to shrink

    private bool hasPlayed;
    private Vector3 initialScale;

    public Transform m_fire;

    void Start()
    {
        initialScale = transform.localScale;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag(targetTag) && !hasPlayed)
        {
            Debug.Log("Hit GameObject Name : " + other.gameObject.name);

            hasPlayed = true;

            StartCoroutine(SmoothScaleToZero());
        }
    }

    IEnumerator SmoothScaleToZero()
    {
        float time = 0f;

        while (time < scaleDuration)
        {
            float t = time / scaleDuration;

            m_fire.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);

            time += Time.deltaTime;
            yield return null;
        }

        m_fire.localScale = Vector3.zero;

        // Optional: disable object after shrinking
        gameObject.SetActive(false);
        onParticleHit.Invoke();
    }
}