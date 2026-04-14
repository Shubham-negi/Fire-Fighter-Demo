using UnityEngine;

public class ParticleCollisionDisableVFX : MonoBehaviour
{
    [Header("Target VFX Object")]
    public GameObject vfxObject;

    [Header("Options")]
    public bool disableOnHit = true;   // true = disable object, false = just stop particle

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle hit: " + other.name);

        if (vfxObject != null)
        {
            if (disableOnHit)
            {
                vfxObject.SetActive(false);
            }
            else
            {
                ParticleSystem ps = vfxObject.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Stop();
                }
            }
        }
    }
}