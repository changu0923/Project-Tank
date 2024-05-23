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

    // ��ũ��Ʈ ����� bool ����
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

    // ������ �߻��մϴ�.
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

    // ���������� �߻��մϴ�.
    private void SubGunFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, subGunPoint.position, subGunPoint.rotation);
        bullet.GetComponent<Shell>().Fire();
    }

    #region Coroutine

    IEnumerator ReloadMainGun(float reloadTime)
    {
        currentReloadTime = reloadTime; // �ʱ�ȭ
        if (photonView.IsMine)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        while (currentReloadTime > 0f)
        {
            currentReloadTime -= Time.deltaTime; // ���� ������ ���� ��Ȳ ������Ʈ
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
    // ���� ���� Ű �Է�
    private void OnMouseLeft(InputValue key) 
    {
        if (scriptOn == false) return;

        if(key.isPressed == true)
        {
            photonView.RPC("MainGunFire", RpcTarget.All);
        }
    }

    // �������� Ű �Է�
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
