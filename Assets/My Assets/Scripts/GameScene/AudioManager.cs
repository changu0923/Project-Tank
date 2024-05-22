using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [Header("GameScene")]
    public AudioClip hitMarkerEffect;

    #region Singleton
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    instance = obj.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayerHitEffect()
    {
        PlayAudio(hitMarkerEffect);
    }
    public void PlayAudio(AudioClip audio)
    {
        AudioClip clip = audio;
        audioSource.PlayOneShot(clip);
    }
}
