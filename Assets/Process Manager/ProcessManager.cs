using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProcessManager : MonoBehaviour
{
    [Header("Global UI References")]
    public TextMeshProUGUI uiText;
    public AudioSource audioSource;

    private List<Process> processes = new List<Process>();
    private int currentProcessIndex = 0;

    void Start()
    {
        processes.Clear();
        foreach (Transform child in transform)
        {
            Process process = child.GetComponent<Process>();
            if (process != null)
            {
                processes.Add(process);
            }
        }

        StartNextProcess();
    }

    public void StartNextProcess()
    {
        if (currentProcessIndex < processes.Count)
        {
            processes[currentProcessIndex].StartProcess(OnProcessComplete, uiText, audioSource);
        }
        else
        {
            Debug.Log("All processes completed.");
            if (uiText != null)
            {
                uiText.gameObject.SetActive(false);
            }
        }
    }

    private void OnProcessComplete()
    {
        currentProcessIndex++;
        StartNextProcess();
    }
}
