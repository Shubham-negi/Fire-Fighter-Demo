using System.Collections;
using UnityEngine;

public class WaterSequentialActivatorManager : MonoBehaviour
{
    public float delayBetweenObjects = 0.1f;
    public ParticleSystem m_tankForm;

    public Transform foamObject;  
    public float foamScaleSpeed = 0.05f;

    public Material m_tankMaterial;


    public AudioSource m_WaterSoundActive;
    private bool IsWaterSoundActive;

  

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
IEnumerator FadeTankMaterial(float duration)
{
    // Make Transparent (important full setup)
    m_tankMaterial.SetFloat("_Surface", 1);
    m_tankMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
    m_tankMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    m_tankMaterial.SetInt("_ZWrite", 0);
    m_tankMaterial.renderQueue = 3000;

    yield return null;

    // Fade
    float time = 0f;

    Color color = m_tankMaterial.color;
    color.a = 1f;
    m_tankMaterial.color = color;

    while (time < duration)
    {
        time += Time.deltaTime;
        float t = time / duration;

        color.a = Mathf.Lerp(1f, 0f, t);
        m_tankMaterial.color = color;

        yield return null;
    }

    color.a = 0f;
    m_tankMaterial.color = color;
}
    
}