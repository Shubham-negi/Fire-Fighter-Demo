using System.Collections;
using System.Collections.Generic;
using HighlightPlus;
using UnityEngine;
using UnityEngine.UI;

public class WaterFireExtinguisherManager : MonoBehaviour
{

    public static WaterFireExtinguisherManager Instance;

    public FireFighterSoundManager fireFighterSoundManager;
    public GameObject m_startPanel;
    public GameObject m_mashall;
    public GameObject m_oilBarrel;
    public GameObject m_waterContainer;
    public GameObject m_fireExtinguisher;

    public GameObject m_fireTriangleUIPanel;
    public GameObject m_correctCanvas;

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
        Debug.Log("You did it!");
        if (!isMashallHighlighted)
        {
            m_oilBarrel.GetComponent<HighlightEffect>().enabled = true;
            isMashallHighlighted = true;
        }
    }

    public void OilBarrelTouchMashall()
    {
        Debug.Log("You did it!");
        m_oilBarrel.GetComponent<HighlightEffect>().enabled = false;
        fireFighterSoundManager.PlaySound(2);

    }



    private void PickWaterContainer()
    {
        Debug.Log("You did it!");
        m_waterContainer.GetComponent<HighlightEffect>().enabled = true;
        fireFighterSoundManager.PlaySound(3);

    }
    private void AfterUsingWaterContainer()
    {
        Debug.Log("You did it!");
        fireFighterSoundManager.PlaySound(4);
        //water particles going down and settling in the bottom of the drum

    }

    private void FireTriangleUI()
    {
        Debug.Log("You did it!");
        m_fireTriangleUIPanel.SetActive(true);
        fireFighterSoundManager.PlaySound(5);

    }
    public void OnClickNextButton()
    {
        m_fireTriangleUIPanel.SetActive(false);
        m_fireExtinguisher.GetComponent<HighlightEffect>().enabled = true;
        fireFighterSoundManager.PlaySound(6);

    }

    public void AfterFoamRelease()
    {

        fireFighterSoundManager.PlaySound(7);

    }


    public void AgainFireTriangleUI()
    {

        m_fireTriangleUIPanel.SetActive(true);
        fireFighterSoundManager.PlaySound(8);

    }

    public void checkCanvas()
    {

        m_correctCanvas.SetActive(true);
        fireFighterSoundManager.PlaySound(9);

    }
}
