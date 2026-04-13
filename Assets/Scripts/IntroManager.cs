using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 👈 Important

public class IntroManager : MonoBehaviour
{
    public FireFighterSoundManager fireFighterSoundManager;
    public HighlightObject fuleHighlightObject;
    public HighlightObject oxygenHighlightObject;
    public HighlightObject heatHighlightObject;

    [SerializeField] private GameObject m_fire;


    [SerializeField] private int m_currentIndexPanel;
    [SerializeField] private CanvasGroup[] m_panel;

    [Range(1, 10)]
    [SerializeField] private float m_currentPanelSmoot;

    [SerializeField] private Button m_nextButton;

    [SerializeField] private string m_nextSceneName;

    void Start()
    {
        ChackAudioSourcePlaying();
        m_nextButton.onClick.AddListener(onClickNextButton);
    }

    void Update()
    {
        PanelManager(m_currentIndexPanel);
    }

    private void PanelManager(int currentIndex)
    {
        for (int i = 0; i < m_panel.Length; i++)
        {
            if (currentIndex == i)
            {
                m_panel[i].alpha = Mathf.Lerp(m_panel[i].alpha, 1, m_currentPanelSmoot * Time.deltaTime);
                m_panel[i].blocksRaycasts = true;
            }
            else
            {
                m_panel[i].alpha = Mathf.Lerp(m_panel[i].alpha, 0, m_currentPanelSmoot * Time.deltaTime);
                m_panel[i].blocksRaycasts = false;
            }
        }
    }


    public void onClickNextButton()
    {
        m_nextButton.gameObject.SetActive(false);
        if (m_currentIndexPanel < m_panel.Length - 1)
        {
            m_currentIndexPanel++;
            ChackAudioSourcePlaying();
        }
        else
        {
            SceneManager.LoadScene(m_nextSceneName);
        }
    }
    private void ChackAudioSourcePlaying()
    {
        StartCoroutine(ChackAudioPlaying());
    }

    private IEnumerator ChackAudioPlaying()
    {
        fireFighterSoundManager.PlaySound(m_currentIndexPanel);
        if (m_currentIndexPanel == 3)
        {
            HighlightObjectFule();
        }
        if (m_currentIndexPanel == 4)
        {
            HighlightObjectOxygen();
        }
        if (m_currentIndexPanel == 5)
        {
            HighlightObjectHeat();
        }
        if (m_currentIndexPanel == 6)
        {
            m_fire.SetActive(true);
        }
        yield return new WaitWhile(() => fireFighterSoundManager.m_audioSource.isPlaying);
        fireFighterSoundManager.PlaySound(7);
        HighlightObjectOff();
        m_nextButton.gameObject.SetActive(true);
    }

    private void HighlightObjectFule()
    {
        fuleHighlightObject.enabled = true;
        fuleHighlightObject.HighlightObjectActive();
    }
    private void HighlightObjectOxygen()
    {
        oxygenHighlightObject.enabled = true;
        oxygenHighlightObject.HighlightObjectActive();

    }
    private void HighlightObjectHeat()
    {
        heatHighlightObject.enabled = true;
        heatHighlightObject.HighlightObjectActive();

    }


    private void HighlightObjectOff()
    {
        fuleHighlightObject.StopHighlight();
        oxygenHighlightObject.StopHighlight();
        heatHighlightObject.StopHighlight();
    }
}