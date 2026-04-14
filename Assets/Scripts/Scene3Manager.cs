using System.Collections;
using HighlightPlus;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene3Manager : MonoBehaviour
{



    public FireAndLightningManager fireAndLightningManager;
    public Scene3AudioManager1 fireFighterSoundManager;

    public HighlightEffect m_highlightEffectFireAlarm;
    public BoxCollider m_boxColliderGlass;


    [Header("UI Buttons")]
    [SerializeField] private Button m_startTrainingButton;
    [SerializeField] private Button m_damageNextButton;

    [Header("UI Panels")]
    [SerializeField] private GameObject m_startTrainingPanel;
    [SerializeField] private GameObject m_damagePanel;


    public GameObject ppeKit;

    [Header("Destination Points")]
    public GameObject mcpDestination;

    public GameObject reachPpeKitDestination;

    [Header("Liquid objsects")]
    public GameObject reachWaterCanonDestination;
    public GameObject reachFoamCanonDestination;

        [Header("ENV sounds")]
        public GameObject AlarmCirenSound;




    public void Start()
    {
        fireFighterSoundManager.PlaySound(0); // intro sound
        ChackAudioSourcePlaying();

        m_startTrainingPanel.SetActive(true);
        m_damagePanel.SetActive(false);

        m_startTrainingButton.onClick.AddListener(OnClickStartTrainingButton);
        m_damageNextButton.onClick.AddListener(OnClickNextButton);
    }
    private IEnumerator HandleStartTraining()
    {
        yield return new WaitWhile(() => fireFighterSoundManager.m_audioSource.isPlaying);
        m_startTrainingButton.interactable = true;
    }

    private IEnumerator ChackAudioSourcePlayingSecond()
    {
        yield return new WaitWhile(() => fireFighterSoundManager.m_audioSource.isPlaying);
        m_damagePanel.SetActive(true);
        fireFighterSoundManager.PlaySound(1);
        yield return new WaitWhile(() => fireFighterSoundManager.m_audioSource.isPlaying);
        m_damageNextButton.interactable = true;
    }

    private void OnClickStartTrainingButton()
    {
        fireFighterSoundManager.PlaySound(2); // Start training sound
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
        fireAndLightningManager.StartFire();

        Invoke("ActiveMBPAlarm", 20f); // Activate alarm after 15 seconds

        // moveInCurveXZ.HitTargetDent();
        // StartCoroutine(ChackAudioSourcePlayingThird());

    }

    public void ActiveMBPAlarm()
    {
        fireFighterSoundManager.PlaySound(4);
        m_highlightEffectFireAlarm.enabled = true;
        m_boxColliderGlass.enabled = true;
        mcpDestination.SetActive(true);

    }

    private IEnumerator ChackAudioSourcePlayingFifth()
    {
         yield return new WaitWhile(() => fireFighterSoundManager.m_audioSource.isPlaying);

        fireFighterSoundManager.PlayPrepairForPPEKITVO();
        yield return new WaitForSeconds(5f); // small delay to ensure sound starts

        fireFighterSoundManager.FollowThePathToGetReadyVO();

        reachPpeKitDestination.SetActive(true);
        ppeKit.SetActive(true); // Activate the PPE Kit

        yield return new WaitForSeconds(4f); // small delay to ensure sound starts

AlarmCirenSound.SetActive(true); // Activate the alarm siren sound
        // SceneManager.LoadScene("Scene 1");
    }

    public void OnClickAlarmButton()
    {
        StartCoroutine(ChackAudioSourcePlayingFifth());


    }
}