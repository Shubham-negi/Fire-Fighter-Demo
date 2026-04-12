using UnityEngine;

public class FuelIgnition : MonoBehaviour
{
    public ParticleSystem fireParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FlameWood"))
        {
            fireParticle.gameObject.SetActive(true);
            fireParticle.Play();
            WaterFireExtinguisherManager.Instance.OilBarrelTouchMashall();
        }
    }
}