using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankAttack : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform gunPoint;
    [SerializeField] Transform subGunPoint;
    [SerializeField] GameObject cannonPrefab;
    [SerializeField] GameObject bulletPrefab;

    [Header("Reload Time")]
    [SerializeField] float mainGunReloadTime;

    [Header("VFX")]
    [SerializeField] GameObject cannonFirePrefab;

    [Header("AudioSource")]
    [SerializeField] AudioSource audioSource;
    private AudioClip fireSound;
    private AudioClip reloadSound;

    // 스크립트 제어용 bool 변수
    private bool scriptOn;

    private bool isMainGunReady = true;
    private float currentReloadTime;
    private Transform aimTransfrom;

    public bool ScriptOn { get => scriptOn; set => scriptOn = value; }

    private void Awake()
    {
        fireSound = AudioManager.Instance.fireClip;
        reloadSound = AudioManager.Instance.reloadClip;
    }

    // 주포를 발사합니다.
    [PunRPC]
    private void MainGunFire()
    {

        if (isMainGunReady == true)
        {
            isMainGunReady = false;
            GameObject shell = Instantiate(cannonPrefab, gunPoint.position, gunPoint.rotation);
            if (aimTransfrom == null)
            {
                aimTransfrom = GetComponent<TankTurretMovement>().AimTransform;
            }
            shell.GetComponent<Shell>().SetAimTransform(aimTransfrom);
            shell.GetComponent<Shell>().SetShooterInfo(photonView.Owner.NickName, gunPoint.position);
            GameObject vfx = Instantiate(cannonFirePrefab, gunPoint.position, gunPoint.rotation);
            Destroy(vfx, 2.5f);
            shell.GetComponent<Shell>().Fire();
            AudioManager.Instance.PlayAudio(fireSound, gunPoint, 150f);

            print($"view id : {photonView.ViewID}\n owner :{photonView.Owner.NickName}\nmy nickname : {PhotonNetwork.NickName}");


            if (false == photonView.IsMine)
            {
                StartCoroutine(ReloadMainGunNotMine(mainGunReloadTime));
                return;
            }

            StartCoroutine(ReloadMainGun(mainGunReloadTime));
            UIManager.Instance.playerCanvas.uiReticle.StartReload(mainGunReloadTime);
        }
    }

    // 동축기관총을 발사합니다.
    private void SubGunFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, subGunPoint.position, subGunPoint.rotation);
        bullet.GetComponent<Shell>().Fire();
    }

    #region Coroutine

    IEnumerator ReloadMainGun(float reloadTime)
    {
        currentReloadTime = reloadTime; // 초기화
        if (photonView.IsMine)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        while (currentReloadTime > 0f)
        {
            currentReloadTime -= Time.deltaTime; // 현재 재장전 진행 상황 업데이트
            if (currentReloadTime < 0f)
            {
                currentReloadTime = 0f;
            }
            yield return null;
        }
        isMainGunReady = true;
    }

    IEnumerator ReloadMainGunNotMine(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        isMainGunReady = true;
    }

    #endregion

    #region InputSystem
    // 주포 공격 키 입력
    private void OnMouseLeft(InputValue key) 
    {
        if (scriptOn == false) return;

        if(key.isPressed == true)
        {
            photonView.RPC("MainGunFire", RpcTarget.All);
        }
    }

    // 동축기관총 키 입력
    private void OnSubAttack(InputValue key)
    {
        if (scriptOn == false) return;

        if (key.isPressed == true)
        {
            SubGunFire();
        }
    }
    #endregion;
}
