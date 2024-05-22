using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    [Header("GameScene")]
    public AudioClip hitMarkerEffect;

    [Header("Tank")]
    public AudioClip turretSound;
    public AudioClip fireClip;
    public AudioClip reloadClip;
    public AudioClip penetratedClip;
    public AudioClip deflectedClip;
    public AudioClip explosionClip;

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
        Debug.Log($"Instantiate AudioObjet on [{positon.position}]");
    }
}
