using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1Manager : MonoBehaviour
{
    public Scene3AudioManager fireFighterSoundManager;
    public int targetCount = 6;
    private int currentCount = 0;
    private bool isTriggered = false;

    public void Start()
    {
        StartCoroutine(InsVoiceOverDelay());
    }

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
          fireFighterSoundManager.PlaySound(1);
           yield return new WaitWhile(() => fireFighterSoundManager.m_audioSource.isPlaying);
       // yield return new WaitForSeconds(1f);

        // SceneManager.LoadScene("Scene 2"); 
         SceneManager.LoadScene("Main Scene");
    }


    IEnumerator InsVoiceOverDelay()
    {
        Debug.Log("Waiting for 1 second before playing the intro voice over...     ");
        yield return new WaitForSeconds(2f);
        fireFighterSoundManager.PlaySound(0);
    }
}