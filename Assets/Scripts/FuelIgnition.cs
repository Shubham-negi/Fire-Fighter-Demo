using UnityEngine;

public class FuelIgnition : MonoBehaviour
{
    public ParticleSystem fireParticle;
private bool hasTriggered = false;
  private void OnTriggerEnter(Collider other)
{
    if (hasTriggered) return; // ⛔ already triggered

    if (other.CompareTag("FlameWood"))
    {
        hasTriggered = true; // ✅ lock it

        fireParticle.gameObject.SetActive(true);
        fireParticle.Play();

        WaterFireExtinguisherManager.Instance.OilBarrelTouchMashall();
    }
}
}