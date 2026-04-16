using System.Collections;
using System.Collections.Generic;
using HighlightPlus;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HurricaneVR.Framework.Core;

public class WaterFireExtinguisherManager : MonoBehaviour
{

    public static WaterFireExtinguisherManager Instance;

    public Scene3AudioManager fireFighterSoundManager;
    public GameObject m_startPanel;
    public GameObject m_mashall;
    public GameObject m_oilBarrel;
    public GameObject m_waterContainer;
    public GameObject m_fireExtinguisher;

     public GameObject m_waterContainerArrow;
    public GameObject m_fireExtinguisherArrow;

    public GameObject m_fireTriangleUIPanel;
    public float speed = 1f;
    public Transform m_drumWater;
    public Transform m_drumFullOil;
    public Transform m_drumHalfOil;

    public Transform m_drumFoam;


    public ParticleSystem m_dramParticleSystem;

    private bool isMashallHighlighted = false;


    [Header("UI Buttons")]
    [SerializeField] private Button m_startButton;
    [SerializeField] private Button m_nextButton;


    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        StartCoroutine(InsVoiceOverDelay());
        m_startPanel.SetActive(true);
        m_startButton.onClick.AddListener(OnClickStartButton);
        m_nextButton.onClick.AddListener(OnClickNextButton);
                m_mashall.GetComponent<HVRGrabbable>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator InsVoiceOverDelay()
    {
        // Debug.Log("Waiting for 1 second before playing the intro voice over...     ");
        yield return new WaitForSeconds(1.5f);
        fireFighterSoundManager.PlaySound(0);
        yield return new WaitWhile(() => fireFighterSoundManager.m_audioSource.isPlaying);
        m_startButton.interactable = true;
        m_mashall.GetComponent<HVRGrabbable>().enabled = true;
    }

    private void OnClickStartButton()
    {
        Debug.Log("Start button clicked! Training started.");
        m_startPanel.SetActive(false);
        Mashall();
    }

    private void Mashall()
    {
        Debug.Log("You did it!");
        fireFighterSoundManager.PlaySound(1);
        m_mashall.GetComponent<HighlightEffect>().enabled = true;
    }

    public void OilBarrel()
    {
        Debug.Log("did it!");
        if (!isMashallHighlighted)
        {
            m_oilBarrel.GetComponent<HighlightEffect>().enabled = true;
            isMashallHighlighted = true;
        }
    }

    public void OilBarrelTouchMashall()
    {
        Debug.Log("You");
        m_oilBarrel.GetComponent<HighlightEffect>().enabled = false;
        fireFighterSoundManager.PlaySound(2);
        StartCoroutine(AfterTouchMashallVoiceOverDelay());
    }

    IEnumerator AfterTouchMashallVoiceOverDelay()
    {
        Debug.Log("AfterTouchMashallVoiceOverDelay");
        yield return new WaitWhile(() => fireFighterSoundManager.m_audioSource.isPlaying);
        PickWaterContainer();
                m_mashall.GetComponent<HVRGrabbable>().enabled = false;

    }

    private void PickWaterContainer()
    {
        Debug.Log("did");
        m_waterContainer.GetComponent<HighlightEffect>().enabled = true;
        m_waterContainerArrow.SetActive(true);
        fireFighterSoundManager.PlaySound(3);
    }

    public void AfterUsingWaterContainer()
    {
        Debug.Log("You did it!");
        fireFighterSoundManager.PlaySound(4);
        //water particles going down and settling in the bottom of the drum
        ScaleZ();

    }

    public void ScaleZ()
    {
        StartCoroutine(ScaleZRoutine());
    }

    IEnumerator ScaleZRoutine()
    {
        m_dramParticleSystem.gameObject.SetActive(true);
        m_dramParticleSystem.Play();
        yield return new WaitForSeconds(1f);
        m_drumWater.gameObject.SetActive(true);
        Vector3 scale = m_drumWater.localScale;
        scale.z = 0f;
        m_drumWater.localScale = scale;

        m_drumFullOil.gameObject.SetActive(false);
        m_drumHalfOil.gameObject.SetActive(true);
        while (m_drumWater.localScale.z < 1f)
        {
            scale = m_drumWater.localScale;
            scale.z += speed * Time.deltaTime;
            scale.z = Mathf.Clamp01(scale.z);
            m_drumWater.localScale = scale;

            yield return null;
        }
        m_dramParticleSystem.Stop();
        m_dramParticleSystem.gameObject.SetActive(false);
        FireTriangleUI();
    }


    private void FireTriangleUI()
    {
        // Debug.Log("You did it!");
        m_fireTriangleUIPanel.SetActive(true);
        fireFighterSoundManager.PlaySound(5);
        StartCoroutine(AfterFireTriangleUIVoiceOverDelay());
    }

    IEnumerator AfterFireTriangleUIVoiceOverDelay()
    {
        yield return new WaitWhile(() => fireFighterSoundManager.m_audioSource.isPlaying);
        m_nextButton.interactable = true;
    }

    public void OnClickNextButton()
    {
        m_fireTriangleUIPanel.SetActive(false);
        m_fireExtinguisher.GetComponent<HighlightEffect>().enabled = true;
        m_fireExtinguisherArrow.SetActive(true);
        fireFighterSoundManager.PlaySound(6);

    }

    public void AfterFoamRelease()
    {
        fireFighterSoundManager.PlaySound(7);
        FoamScaleZ();
    }

    public void FoamScaleZ()
    {
        StartCoroutine(FoamScaleZRoutine());
    }

    IEnumerator FoamScaleZRoutine()
    {
        m_drumFoam.gameObject.SetActive(true);
        m_drumFoam.localScale = Vector3.zero;

        while (m_drumFoam.localScale.x < 1f)
        {
            Vector3 scale = m_drumFoam.localScale;

            float value = scale.x + speed * Time.deltaTime;
            value = Mathf.Clamp01(value);

            m_drumFoam.localScale = new Vector3(value, value, value);

            yield return null;
        }
        AgainFireTriangleUI();
    }

    public void AgainFireTriangleUI()
    {
        m_nextButton.gameObject.SetActive(false);
        m_fireTriangleUIPanel.SetActive(true);
        fireFighterSoundManager.PlaySound(8);
        StartCoroutine(checkCanvasVoiceOverDelay());
    }

    IEnumerator checkCanvasVoiceOverDelay()
    {
        yield return new WaitWhile(() => fireFighterSoundManager.m_audioSource.isPlaying);
        checkCanvas();
    }

    public void checkCanvas()
    {
        fireFighterSoundManager.PlaySound(9);
        StartCoroutine(CompleteTrainingVoiceOverDelay());
    }

    IEnumerator CompleteTrainingVoiceOverDelay()
    {
        yield return new WaitWhile(() => fireFighterSoundManager.m_audioSource.isPlaying);
        fireFighterSoundManager.PlaySound(10);
        yield return new WaitWhile(() => fireFighterSoundManager.m_audioSource.isPlaying);
        SceneManager.LoadScene("Scene 3");
    }

    public void LoadnextScene()
    {        fireFighterSoundManager.m_audioSource.Stop();

        SceneManager.LoadScene("Scene 3");
    }
}
