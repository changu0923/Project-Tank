using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TankHullMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;

    [Header("Engine")]
    [SerializeField] AudioSource engineAudioSource;
    private AudioClip engineIdleClip;
    private AudioClip engineActiveClip;
    Coroutine engineSoundCoroutine;
    private PhotonView photonView;

    private float pitchChangeSpeed = 0.1f;
    private float minPitch = 0.5f;
    private float maxPitch = 1.0f;

    public enum TankStatus
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
        photonView = GetComponent<PhotonView>();
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
        }
        else
        {
            currentStatus = TankStatus.IDLE;
        }

        if (currentStatus != prevStatus)
        {
            photonView.RPC("PlayEngineSoundEffect", RpcTarget.All, currentStatus);
            prevStatus = currentStatus;
        }
    }

    [PunRPC]
    public void PlayEngineSoundEffect(TankStatus status)
    {
        if (engineSoundCoroutine != null)
        {
            StopCoroutine(engineSoundCoroutine);
        }

        if (status == TankStatus.IDLE)
        {
            engineSoundCoroutine = StartCoroutine(SoundEngineMoveToIdle());
        }
        else if (status == TankStatus.MOVING)
        {
            engineSoundCoroutine = StartCoroutine(SoundEngineIdleToMove());
        }
        else    // (status == TankStatus.STOP)
        {
            // TODO : 퍼진 소리 출력
        }
    }

    #region SoundControl
    IEnumerator SoundEngineIdleToMove()
    {
        engineAudioSource.clip = engineActiveClip;
        engineAudioSource.Play();
        float targetPitch = maxPitch;
        while (engineAudioSource.pitch < targetPitch)
        {
            engineAudioSource.pitch += pitchChangeSpeed * 2f * Time.deltaTime;
            yield return null;
        }
        engineAudioSource.pitch = targetPitch;
    }

    IEnumerator SoundEngineMoveToIdle()
    {
        float targetPitch = minPitch;
        while (engineAudioSource.pitch > targetPitch)
        {
            engineAudioSource.pitch -= pitchChangeSpeed * 10f * Time.deltaTime;
            yield return null;
        }
        engineAudioSource.pitch = targetPitch;
        engineAudioSource.clip = engineIdleClip;
        engineAudioSource.Play();
    }
    #endregion
}