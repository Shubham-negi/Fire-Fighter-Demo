using System.Collections;
using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    private Material mat;
    private Color originalColor;

    private Coroutine highlightRoutine;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        originalColor = mat.GetColor("_EmissionColor");
    }

    public void HighlightObjectActive()
    {
        if (highlightRoutine == null)
        {
            highlightRoutine = StartCoroutine(CheckHighlightObject());
        }
    }

    public void StopHighlight()
    {
        if (highlightRoutine != null)
        {
            StopCoroutine(highlightRoutine);
            highlightRoutine = null;
        }

        HighlightOff();
    }

    private IEnumerator CheckHighlightObject()
    {
        while (true)
        {
            HighlightOn();
            yield return new WaitForSeconds(0.5f);

            HighlightOff();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void HighlightOn()
    {
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.yellow * 0.5f);
    }

    public void HighlightOff()
    {
        mat.SetColor("_EmissionColor", originalColor);
    }
}