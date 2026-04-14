using UnityEngine;

public class FireExtinguisherController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform handleTransform;
    [SerializeField] private ParticleSystem fireParticle;

    [Header("Settings")]
    [SerializeField] private float pressedAngle = 30f;
    [SerializeField] private float rotationSpeed = 10f;

    private bool canUseExtinguisher = false;
    private bool isTriggerPressed = false;

    private Quaternion handleDefaultRotation;
    private Quaternion handlePressedRotation;

    private void Start()
    {
        if (handleTransform != null)
        {
            handleDefaultRotation = handleTransform.localRotation;
            handlePressedRotation = Quaternion.Euler(
                pressedAngle,
                handleTransform.localEulerAngles.y,
                handleTransform.localEulerAngles.z
            );
        }

        if (fireParticle != null)
        {
            fireParticle.Stop();
        }
    }

    private void Update()
    {
        if (!canUseExtinguisher || handleTransform == null)
            return;

        Quaternion targetRotation = isTriggerPressed
            ? handlePressedRotation
            : handleDefaultRotation;

        handleTransform.localRotation = Quaternion.Lerp(
            handleTransform.localRotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    // 🔓 Called when pin is pulled
    public void OnPinPulled()
    {
        canUseExtinguisher = true;
    }

    // 🔫 Called when trigger is pressed
    public void PressTrigger()
    {
        if (!canUseExtinguisher)
            return;

        isTriggerPressed = true;

        if (fireParticle != null && !fireParticle.isPlaying)
        {
            fireParticle.Play();
        }
    }

    // ✋ Called when trigger is released
    public void ReleaseTrigger()
    {
        isTriggerPressed = false;

        if (fireParticle != null && fireParticle.isPlaying)
        {
            fireParticle.Stop();
        }
    }
}
