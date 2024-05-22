using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio3DSpace : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip audioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip audioClip, float distance)
    {
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = distance;
        float clipTime = 0f;
        if (audioClip != null) 
        {
            clipTime = audioClip.length;
            StartCoroutine(DestoryAfterAudioEnd(clipTime));
            audioSource.PlayOneShot(audioClip);
        }
    }

    IEnumerator DestoryAfterAudioEnd(float _t)
    {
        float lifeTime = _t + 0.2f;
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}