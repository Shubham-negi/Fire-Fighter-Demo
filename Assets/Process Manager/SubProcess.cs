using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SubProcess : MonoBehaviour
{
    [Header("Auto Complete Settings")]
    public float autoCompleteTime = 0f;

    [Header("Text and Audio")]
    [TextArea]
    public string textToShow;
    public AudioClip audioClip;
    
    [Tooltip("If true, subprocess will complete automatically when audio finishes")]
    public bool audioCompleteSubProcess = false;

    [Header("Events")]
    public UnityEvent OnEnableEvent;
    public UnityEvent OnCompleteEvent;

    [Header("Enable / Disable Objects")]
    public List<GameObject> enableObjects = new List<GameObject>();
    public List<GameObject> disableObjects = new List<GameObject>();

    [Header("Enable / Disable Colliders")]
    public List<Collider> colliders = new List<Collider>();

    [Header("Highlight Objects")]
    public List<GameObject> highlightObjects = new List<GameObject>();

    [Header("Destination Trigger")]
    public GameObject DestinationPoint;
    public string playerTag = "Player";

    [HideInInspector] public bool isStarted;
    [HideInInspector] public bool isCompleted;

    private System.Action onSubProcessCompleteCallback;
    private TextMeshProUGUI sharedUIText;
    private AudioSource sharedAudioSource;
    private Coroutine audioWaitCoroutine;

    public void StartSubProcess(
        System.Action onComplete,
        TextMeshProUGUI uiText,
        AudioSource audioSource)
    {
        CancelInvoke();
        if (audioWaitCoroutine != null)
        {
            StopCoroutine(audioWaitCoroutine);
            audioWaitCoroutine = null;
        }

        isStarted = true;
        isCompleted = false;

        onSubProcessCompleteCallback = onComplete;
        sharedUIText = uiText;
        sharedAudioSource = audioSource;

        gameObject.SetActive(true);

        if (sharedUIText != null)
        {
            sharedUIText.gameObject.SetActive(true);
            sharedUIText.text = textToShow;
            Debug.Log($"[SubProcess] {name} started - Text: {textToShow}");
        }

        foreach (var go in enableObjects)
            if (go != null) go.SetActive(true);

        foreach (var go in disableObjects)
            if (go != null) go.SetActive(false);

        foreach (var col in colliders)
            if (col != null) col.enabled = true;

        foreach (var h in highlightObjects)
            if (h != null) h.SetActive(true);

        if (DestinationPoint != null)
            DestinationPoint.SetActive(true);

        OnEnableEvent?.Invoke();

        if (audioClip != null && sharedAudioSource != null)
        {
            sharedAudioSource.Stop();
            sharedAudioSource.clip = audioClip;
            sharedAudioSource.Play();
            
            Debug.Log($"[SubProcess Audio] Playing: {audioClip.name} (Length: {audioClip.length}s)");

            if (audioCompleteSubProcess)
            {
                audioWaitCoroutine = StartCoroutine(WaitForAudioAndComplete());
                Debug.Log($"[SubProcess] Will auto-complete after audio finishes");
            }
        }

        if (autoCompleteTime > 0f && !audioCompleteSubProcess)
        {
            Invoke(nameof(CompleteSubProcess), autoCompleteTime);
            Debug.Log($"[SubProcess] Will auto-complete in {autoCompleteTime}s");
        }
    }

    IEnumerator WaitForAudioAndComplete()
    {
        if (audioClip == null || sharedAudioSource == null)
            yield break;

        float audioDuration = audioClip.length;
        float elapsed = 0f;

        while (elapsed < audioDuration && sharedAudioSource.isPlaying)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        Debug.Log($"[SubProcess Audio] Finished playing {audioClip.name} - Auto-completing subprocess");
        CompleteSubProcess();
    }

    public void CompleteSubProcess()
    {
        if (isCompleted)
            return;

        isCompleted = true;
        isStarted = false;
        CancelInvoke();
        
        if (audioWaitCoroutine != null)
        {
            StopCoroutine(audioWaitCoroutine);
            audioWaitCoroutine = null;
        }

        Debug.Log($"[SubProcess] {name} completed");

        foreach (var h in highlightObjects)
            if (h != null) h.SetActive(false);

        foreach (var col in colliders)
            if (col != null) col.enabled = false;

        if (DestinationPoint != null)
            DestinationPoint.SetActive(false);

        OnCompleteEvent?.Invoke();

        onSubProcessCompleteCallback?.Invoke();
    }
}
