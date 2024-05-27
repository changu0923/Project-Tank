using Photon.Pun;
using System.Collections;
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
    Coroutine engineSoundCoroutine;
    private PhotonView photonview;

    private float pitchChangeSpeed = 0.1f;
    private float minPitch = 0.5f;
    private float maxPitch = 1.0f;

    private enum TankStatus
    {
        IDLE,
        MOVING,
        STOP,
    }

    private TankStatus currentStatus;
    private TankStatus prevStatus;

    float x;
    float z;
    private Rigidbody rb;

    public float X { get => x; }
    public float Z { get => z; }
    private TankStatus CurrentStatus { get => currentStatus; set => currentStatus = value; }
    private TankStatus PrevStatus { get => prevStatus; set => prevStatus = value; }

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
        currentStatus = TankStatus.IDLE;
        prevStatus = CurrentStatus;

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
        EngineSoundEffect(z, x);
    }

    void Move()
    {
        if (Z != 0f)
        {
            Vector3 moveDirection = new Vector3(0f, 0f, Z) * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(transform.position + transform.TransformDirection(moveDirection));
        }
    }
    void Rotate()
    {
        if (X != 0f)
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
            currentStatus = TankStatus.MOVING;
            if(currentStatus != prevStatus)
            {
                PlayEngineSoundEffect();
            }
        }
        else
        {
            currentStatus = TankStatus.IDLE;
            if(currentStatus != prevStatus) 
            {
                PlayEngineSoundEffect();
            }
        }
        prevStatus = currentStatus;
    }

    [PunRPC]
    public void PlayEngineSoundEffect()
    {
        if (engineSoundCoroutine != null)
        {
            StopCoroutine(engineSoundCoroutine);
        }

        if (currentStatus == TankStatus.IDLE)
        {
            // TODO : 엔진음 작아지다가 Idle 엔진음 출력
            engineSoundCoroutine = StartCoroutine(SoundEngineMoveToIdle());
            Debug.Log("PlayEngineSoundEffect() : Moving -> Idle");
        }
        else if (currentStatus == TankStatus.MOVING)
        {
            // TODO : Idle에서 Moving 엔진음 출력
            engineSoundCoroutine = StartCoroutine(SoundEngineIdleToMove());
            Debug.Log("PlayEngineSoundEffect() : Idle -> Moving");
        }
        else    // (currentStatus == TankStatus.STOP)
        {
            // TODO : 퍼진 소리 출력
            Debug.Log("PlayEngineSoundEffect() : Stopped");
        }
    }

    #region SoundControl
    IEnumerator SoundEngineIdleToMove()
    {
        float targetPitch = maxPitch;
        while (engineAudioSource.pitch < targetPitch)
        {
            engineAudioSource.pitch += pitchChangeSpeed * Time.deltaTime;
            yield return null;
        }
        engineAudioSource.pitch = targetPitch;
    }
    IEnumerator SoundEngineMoveToIdle()
    {
        float targetPitch = minPitch;
        while (engineAudioSource.pitch > targetPitch)
        {
            engineAudioSource.pitch -= pitchChangeSpeed * Time.deltaTime;
            yield return null;
        }
        engineAudioSource.pitch = targetPitch;
    }
    #endregion
}
