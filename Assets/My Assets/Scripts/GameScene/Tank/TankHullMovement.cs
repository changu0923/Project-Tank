using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class TankHullMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;

    [Header("Engine")]
    [SerializeField] AudioSource engineAudioSource;
    private AudioClip engineIdleClip;
    private AudioClip engineActiveClip;

    private PhotonView photonview;

    float x;
    float z;
    private Rigidbody rb;    

    public float X { get => x; }
    public float Z { get => z; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        engineActiveClip = AudioManager.Instance.engineActiveClip;
        engineIdleClip = AudioManager.Instance.engineIdleClip;  
        photonview = GetComponent<PhotonView>();
    }

    void Start()
    {
        InitEngine();

        rb.velocity = Vector3.zero;
        x = 0;
        z = 0;
    }

    void Update()
    {
        GetXZ();
    }

    private void FixedUpdate()
    {       
        Move();
        Rotate();
    }  

    private void InitEngine()
    {
        engineAudioSource.rolloffMode = AudioRolloffMode.Linear;
        engineAudioSource.spatialBlend = 1f;
        engineAudioSource.maxDistance = 100f;
        engineAudioSource.loop = true;

        if (engineAudioSource.clip == null)
        {
            engineAudioSource.clip = engineIdleClip;
        }
        if (engineAudioSource.isPlaying == false)
        {
            engineAudioSource.Play();
        }
    }

    void GetXZ()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        EngineSoundEffect(z,x);
    }

    void Move()
    {
        if(Z != 0f) 
        {
            Vector3 moveDirection = new Vector3(0f, 0f, Z) * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(transform.position + transform.TransformDirection(moveDirection));
        }
    }
    void Rotate()
    {
        if(X != 0f) 
        {
            Vector3 rotation = new Vector3(0f, x * rotationSpeed * Time.deltaTime, 0f);
            Quaternion deltaRotation = Quaternion.Euler(rotation);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }

    public void EngineSoundEffect(float moveZ, float rotateX)
    {
        if (moveZ != 0 || rotateX != 0)
        {
            if (engineAudioSource.clip != engineActiveClip)
            {
                engineAudioSource.clip = engineActiveClip;
                engineAudioSource.Play();
            }

            float pitchZ = Mathf.Clamp(Mathf.Abs(moveZ), 0f, 1f);
            float pitchX = Mathf.Clamp(Mathf.Abs(rotateX), 0f, 1f);
            engineAudioSource.pitch = Mathf.Max(pitchZ, pitchX);
        }
        else
        {
            if (engineAudioSource.clip != engineIdleClip)
            {
                engineAudioSource.clip = engineIdleClip;
                engineAudioSource.Play();
            }
            engineAudioSource.pitch = 1f;
        }
    }
}
