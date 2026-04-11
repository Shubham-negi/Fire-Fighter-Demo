using System.Collections;
using HighlightPlus;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{

    public MoveInCurveXZ moveInCurveXZ;

    public HighlightEffect m_highlightEffectFireAlarm;
    public BoxCollider m_boxColliderGlass;


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
    private IEnumerator HandleStartTraining()
    {
        yield return new WaitWhile(() => SoundManager.Instance.audioSource.isPlaying);
        m_startTrainingButton.interactable = true;
    }

    private IEnumerator ChackAudioSourcePlayingSecond()
    {
        yield return new WaitWhile(() => SoundManager.Instance.audioSource.isPlaying);
        m_damagePanel.SetActive(true);
        SoundManager.Instance.PlaySound(2);
        yield return new WaitWhile(() => SoundManager.Instance.audioSource.isPlaying);
        m_damageNextButton.interactable = true;
    }

    private void OnClickStartTrainingButton()
    {
        SoundManager.Instance.PlaySound(1);
        m_startTrainingPanel.SetActive(false);
        StartCoroutine(ChackAudioSourcePlayingSecond());
    }

    private void ChackAudioSourcePlaying()
    {
        StartCoroutine(HandleStartTraining());
    }



    private void OnClickNextButton()
    {
        m_damagePanel.SetActive(false);
        moveInCurveXZ.HitTargetDent();
       // StartCoroutine(ChackAudioSourcePlayingThird());

    }

    public void ActiveMBPAlarm()
    {
        SoundManager.Instance.PlaySound(4);
        m_highlightEffectFireAlarm.enabled = true;
        m_boxColliderGlass.enabled = true;  
    }

    private IEnumerator ChackAudioSourcePlayingFifth()
    {
       // yield return new WaitWhile(() => SoundManager.Instance.audioSource.isPlaying);
        yield return new WaitForSeconds(5f); // small delay to ensure sound starts
        SceneManager.LoadScene("Scene 1");
    }

     public void OnClickAlarmButton()
    {
        SoundManager.Instance.PlaySound(5); 
         StartCoroutine(ChackAudioSourcePlayingFifth());
    }
}