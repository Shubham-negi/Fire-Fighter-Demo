using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroSceneManager : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button m_startTrainingButton;
    [SerializeField] private Button m_damageNextButton;

    [Header("UI Panels")]
    [SerializeField] private GameObject m_startTrainingPanel;
    [SerializeField] private GameObject m_damagePanel;

    void Start()
    {
        SoundManager.Instance.PlaySound(0); // intro sound
        ChackAudioSourcePlaying();

        m_startTrainingPanel.SetActive(true);
        m_damagePanel.SetActive(false);

        m_startTrainingButton.onClick.AddListener(OnClickStartTrainingButton);
        m_damageNextButton.onClick.AddListener(OnClickNextButton);
    }

    private void OnClickStartTrainingButton()
    {
        SoundManager.Instance.PlaySound(1);
    }

    private void ChackAudioSourcePlaying()
    {
        StartCoroutine(HandleStartTraining());
    }


    private IEnumerator HandleStartTraining()
    {
        // wait until sound finishes
        yield return new WaitWhile(() => SoundManager.Instance.audioSource.isPlaying);
        m_startTrainingButton.interactable = true;
        m_startTrainingPanel.SetActive(false);
        // m_damagePanel.SetActive(true);

    }

    private void OnClickNextButton()
    {

    }
}