using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
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

    // 스크립트 제어용 bool 변수
    private bool scriptOn;

    private bool isMainGunReady = true;
    private float currentReloadTime;
    private Transform aimTransfrom;

    public bool ScriptOn { get => scriptOn; set => scriptOn = value; }

    // 주포를 발사합니다.
    [PunRPC]
    private void MainGunFire()
    {
        if (isMainGunReady == true)
        {
            isMainGunReady=false;
            GameObject shell = Instantiate(cannonPrefab, gunPoint.position, gunPoint.rotation);           
            if(aimTransfrom == null )
            {
                aimTransfrom = GetComponent<TankTurretMovement>().AimTransform;
            }
            shell.GetComponent<Shell>().SetAimTransform(aimTransfrom);
            shell.GetComponent<Shell>().SetShooterInfo(photonView.Owner.NickName, gunPoint.position);
            GameObject vfx = Instantiate(cannonFirePrefab, gunPoint.position, gunPoint.rotation);
            Destroy(vfx, 2.5f);
            shell.GetComponent<Shell>().Fire();
            StartCoroutine(ReloadMainGun(mainGunReloadTime));
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
