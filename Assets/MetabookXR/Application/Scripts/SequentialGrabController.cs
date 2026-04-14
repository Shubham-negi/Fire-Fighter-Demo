using UnityEngine;
using UnityEngine.Events;
using HurricaneVR.Framework.Core;

public class SequentialGrabController : MonoBehaviour
{
    [Header("Hurricane VR Grabbables (In Order)")]
    public HVRGrabbable[] grabbableSequence;

    [Header("Events")]
    public UnityEvent OnSequenceComplete;

    private int currentIndex = 0;
    private bool currentGrabbableActive = false;

    private void Start()
    {
        DisableAllGrabbables();
    }

    private void DisableAllGrabbables()
    {
        for (int i = 0; i < grabbableSequence.Length; i++)
        {
            if (grabbableSequence[i] != null)
                grabbableSequence[i].enabled = false;
        }
    }

    private void EnableCurrentGrabbable()
    {
        if (currentIndex < grabbableSequence.Length)
        {
            if (grabbableSequence[currentIndex] != null)
            {
                grabbableSequence[currentIndex].enabled = true;
                currentGrabbableActive = true;
            }
        }
        else
        {
            OnSequenceComplete?.Invoke();
        }
    }

    // Call this to turn on the current grabbable.
    public void StartSequence()
    {
        if (currentIndex >= grabbableSequence.Length)
        {
            OnSequenceComplete?.Invoke();
            return;
        }

        DisableAllGrabbables();
        currentGrabbableActive = false;
        EnableCurrentGrabbable();
    }

    // Call this when the current task is completed.
    public void CompleteCurrentTask()
    {
        if (currentIndex >= grabbableSequence.Length || !currentGrabbableActive)
            return;

        if (grabbableSequence[currentIndex] != null)
            grabbableSequence[currentIndex].enabled = false;

        currentGrabbableActive = false;
        currentIndex++;

        if (currentIndex >= grabbableSequence.Length)
        {
            OnSequenceComplete?.Invoke();
        }
    }

    // Optional reset: clears progress and disables everything.
    public void ResetSequence()
    {
        currentIndex = 0;
        currentGrabbableActive = false;
        DisableAllGrabbables();
    }
}
