using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1Manager : MonoBehaviour
{
    public int targetCount = 6;
    private int currentCount = 0;
    private bool isTriggered = false;

    // Call this function whenever needed
    public void RegisterAction()
    {
        if (isTriggered) return; // prevent extra calls after completion

        currentCount++;
        Debug.Log("Count: " + currentCount);

        if (currentCount >= targetCount)
        {
            isTriggered = true;
            StartCoroutine(LoadNextSceneWithDelay());
        }
    }

    IEnumerator LoadNextSceneWithDelay()
    {
        Debug.Log("All actions done. Loading Scene2 in 5 seconds...");
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Scene 2"); // exact scene name
    }
}