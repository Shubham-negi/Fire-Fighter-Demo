using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MoveInCurveXZ : MonoBehaviour
{

    public UnityEvent OnHitTargetDent;

    public GameObject m_fireParticle;
    public Transform target;
    public float duration = 10f;

    private bool isMovingBoll = false;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    public void HitTargetDent()
    {
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            transform.position = Vector3.Lerp(startPos, target.position, t);

            yield return null;
        }

        transform.position = target.position;
        if (!isMovingBoll)
        {
        SoundManager.Instance.PlaySound(3);
        isMovingBoll = true;
        }
        StartCoroutine(ChackAudioSourcePlayingfour());
    }

    private IEnumerator ChackAudioSourcePlayingfour()
    {
        yield return new WaitWhile(() => SoundManager.Instance.audioSource.isPlaying);
        OnHitTargetDent?.Invoke();
        if (m_fireParticle != null)
        {
            m_fireParticle.GetComponent<MoveInCurveXZ>().HitTargetDent();
        }
    }
}