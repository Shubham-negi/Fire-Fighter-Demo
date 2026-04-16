using System.Collections;
using HighlightPlus;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HurricaneVR.Framework.Core;

public class Scene3Manager : MonoBehaviour
{

    Scene3Manager instance;
    public GameObject[] taskListTicks;
    public FireAndLightningManager fireAndLightningManager;
    public Scene3AudioManager1 fireFighterSoundManager;

    public HighlightEffect m_highlightEffectFireAlarm;
    public BoxCollider m_boxColliderGlass;
    public GameObject vHFObject;

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

    private bool alarmTriggered = false;
    private bool vhfTriggered = false;


    public void Start()
    {
        instance = this;
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


        // moveInCurveXZ.HitTargetDent();
        // StartCoroutine(ChackAudioSourcePlayingThird());
        Invoke(nameof(ActiveMBPAlarm), 15f); // Delay of 10 seconds before activating the MBP alarm
    }

    public void ActiveMBPAlarm()
    {
        fireFighterSoundManager.PlaySound(4);
        m_highlightEffectFireAlarm.enabled = true;
        m_boxColliderGlass.enabled = true;
        mcpDestination.SetActive(true);

    }


    private IEnumerator VHFSoundAndAction()
    {
        fireFighterSoundManager.VHFClickPlay();
        yield return new WaitForSeconds(1f); // small delay to ensure sound starts
        fireFighterSoundManager.VHFVOPlay();
        yield return new WaitForSeconds(15f); // small delay to ensure sound starts

        fireFighterSoundManager.PlayPrepairForPPEKITVO();


        yield return new WaitForSeconds(5f); // small delay to ensure sound starts
        fireFighterSoundManager.FollowThePathToGetReadyVO();
        reachPpeKitDestination.SetActive(true);
        ppeKit.SetActive(true); // Activate the PPE Kit
                taskListTicks[1].SetActive(true); // Show tick for "Call the Alarm"

    }

    public void OnClickAlarmButton()
    {
        if (alarmTriggered) return; // ⛔ already called once

        alarmTriggered = true;
        taskListTicks[0].SetActive(true); // Show tick for "Call the Alarm"

        ActiveVHF();
        AlarmCirenSound.SetActive(true);
    }
    public void OnVHFGrabAndTrigger()
    {
        if (vhfTriggered) return; // ⛔ already triggered

        vhfTriggered = true;

        StartCoroutine(VHFSoundAndAction());
    }

    public void ActiveVHF()
    {
        vHFObject.GetComponent<HighlightEffect>().enabled = true;
        vHFObject.GetComponent<HVRGrabbable>().enabled = true;
fireFighterSoundManager.InformFireFighterVO(); // Play the "Inform Fire Fighter" voice over

    }
    // on active and press trigger of VHF, set the X position of all children to 0.42
    public void PressVHFTriggerCall()
    {
        SetChildrenX(0.42f);
    }

    // Set X = 0.35
    public void PressVHFTriggerCallEnd()
    {
        SetChildrenX(0.35f);
    }

    // 🔹 Core function
    private void SetChildrenX(float xValue)
    {

        if (vHFObject == null || vHFObject.transform.childCount == 0) return;

        Transform firstChild = vHFObject.transform.GetChild(0);

        Vector3 pos = firstChild.localPosition;
        pos.x = xValue;
        firstChild.localPosition = pos;
    }

}