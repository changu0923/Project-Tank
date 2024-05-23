using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [Header("UI")]
    public AudioSource uiAudioSource;
    public AudioClip clickClip;
    public AudioClip countDownClip;

    [Header("GameScene")]
    public AudioClip hitMarkerEffect;

    [Header("Tank")]
    public AudioClip turretSound;
    public AudioClip fireClip;
    public AudioClip reloadClip;
    public AudioClip penetratedClip;
    public AudioClip deflectedClip;
    public AudioClip explosionClip;
    public AudioClip engineIdleClip;
    public AudioClip engineActiveClip;

    [Header("Crew Voice")]
    [SerializeField] AudioSource crewAudio;
    public List<AudioClip> pierced = new List<AudioClip>();
    public List<AudioClip> notPierced =new List<AudioClip>();

    [Header("3D Space Audio Object")]
    [SerializeField] GameObject audioElement; 

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
            DontDestroyOnLoad(gameObject);
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

    public void PlayAudio(AudioClip audio, Transform positon, float distance = 150f)
    {
        AudioClip getClip = audio;
        GameObject audioObject = Instantiate(audioElement, positon.position, Quaternion.identity);
        Audio3DSpace audio3D = audioObject.GetComponent<Audio3DSpace>();
        audio3D.PlaySound(getClip, distance);
    }

    public void UIButtonClick()
    {
        AudioClip audioClip = clickClip;
        uiAudioSource.PlayOneShot(audioClip);
    }

    public void UICountDown()
    {
        AudioClip audioClip = countDownClip;
        uiAudioSource.PlayOneShot(audioClip);
    }

    public void CrewAttackSuccess()
    {
        StartCoroutine(CrewAttackSuccessCoroutine());
    }
    public void CrewAttackFailed()
    {
        StartCoroutine(CrewAttackFailedCoroutine());
    }

    IEnumerator CrewAttackSuccessCoroutine()
    {
        yield return new WaitForSeconds(0.75f);
        int index = UnityEngine.Random.Range(0, pierced.Count);
        AudioClip randomClip = pierced[index];
        crewAudio.PlayOneShot(randomClip);
    }
    IEnumerator CrewAttackFailedCoroutine()
    {
        yield return new WaitForSeconds(0.75f);
        int index = UnityEngine.Random.Range(0, notPierced.Count);
        AudioClip randomClip = notPierced[index];
        crewAudio.PlayOneShot(randomClip);
    }
}