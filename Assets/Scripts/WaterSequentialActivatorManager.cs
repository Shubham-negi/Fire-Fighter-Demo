using System.Collections;
using UnityEngine;

public class WaterSequentialActivatorManager : MonoBehaviour
{
    public float delayBetweenObjects = 0.1f;
    public ParticleSystem m_tankForm;

    public Transform foamObject;
    public float foamScaleSpeed = 0.05f;

    public Material m_tankMaterial;

    private Material originalTankMaterial;


    public AudioSource m_WaterSoundActive;
    private bool IsWaterSoundActive;


    void Start()
    {
        if (m_tankMaterial != null)
        {
            originalTankMaterial = new Material(m_tankMaterial); // ✅ REAL COPY
        }
    }

    void OnApplicationQuit()
    {
        if (m_tankMaterial != null && originalTankMaterial != null)
        {
            m_tankMaterial.CopyPropertiesFromMaterial(originalTankMaterial);
        }
    }

    [ContextMenu("Test Fade Only")]
    public void WaterSequentialActivator()
    {
        StartCoroutine(ActivateSequentially());
    }

    IEnumerator ActivateSequentially()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(true);

            for (int j = 0; j < child.childCount; j++)
            {
                GameObject subChild = child.GetChild(j).gameObject;

                if (m_WaterSoundActive != null & !IsWaterSoundActive)
                {
                    m_WaterSoundActive.Play();
                    IsWaterSoundActive = true;
                }
                subChild.SetActive(true);
                yield return new WaitForSeconds(delayBetweenObjects);
            }
        }
        if (m_tankForm != null)
        {
            if (m_tankMaterial != null)
            {
                StartCoroutine(FadeTankMaterial(5f));
            }

            m_tankForm.gameObject.SetActive(true);
            m_tankForm.Play();

            if (foamObject != null)
            {
                FoamActive();
            }
        }
    }


    private void FoamActive()
    {
        StartCoroutine(ScaleFoam());
    }

    IEnumerator ScaleFoam()
    {
        float time = 0;
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = new Vector3(1.37f, 1.37f, 1.37f);

        foamObject.localScale = startScale;

        while (time < 1)
        {
            time += Time.deltaTime * foamScaleSpeed;

            foamObject.localScale = Vector3.Lerp(startScale, targetScale, time);

            yield return null;
        }

        foamObject.localScale = targetScale;
        m_tankForm.Stop();
        m_tankForm.gameObject.SetActive(false);
    }
     
public IEnumerator FadeTankMaterial(float duration)
{
    if (m_tankMaterial == null) yield break;

    float time = 0f;

    Color startColor = m_tankMaterial.color;
    float startAlpha = startColor.a;
    float endAlpha = 0f; // fully transparent

    while (time < duration)
    {
        time += Time.deltaTime;
        float t = time / duration;

        float alpha = Mathf.Lerp(startAlpha, endAlpha, t);

        Color newColor = startColor;
        newColor.a = alpha;

        m_tankMaterial.color = newColor;

        yield return null;
    }

    // Ensure final state
    Color finalColor = startColor;
    finalColor.a = endAlpha;
    m_tankMaterial.color = finalColor;
}

}