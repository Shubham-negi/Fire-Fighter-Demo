using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Process : MonoBehaviour
{
    public List<SubProcess> subProcesses = new List<SubProcess>();
    public int currentSubProcessIndex = 0;
    public System.Action onProcessComplete;

    [HideInInspector] public bool isStarted = false;
    [HideInInspector] public bool isCompleted = false;

    private TextMeshProUGUI sharedUIText;
    private AudioSource sharedAudioSource;

    public void StartProcess(System.Action onComplete, TextMeshProUGUI uiText, AudioSource audioSource)
    {
        isStarted = true;
        isCompleted = false;
        onProcessComplete = onComplete;
        sharedUIText = uiText;
        sharedAudioSource = audioSource;

        subProcesses.Clear();

        foreach (Transform child in transform)
        {
            SubProcess subProcess = child.GetComponent<SubProcess>();
            if (subProcess != null)
            {
                subProcesses.Add(subProcess);
            }
        }

        currentSubProcessIndex = 0;
        StartNextSubProcess();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.RepaintHierarchyWindow();
#endif
    }

    private void StartNextSubProcess()
    {
        if (currentSubProcessIndex < subProcesses.Count)
        {
            subProcesses[currentSubProcessIndex].StartSubProcess(OnSubProcessComplete, sharedUIText, sharedAudioSource);
        }
        else
        {
            isStarted = false;
            isCompleted = true;
            onProcessComplete?.Invoke();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.RepaintHierarchyWindow();
#endif
        }
    }

    private void OnSubProcessComplete()
    {
        currentSubProcessIndex++;
        StartNextSubProcess();
    }
}
