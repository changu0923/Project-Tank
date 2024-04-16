using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankAttack : MonoBehaviour
{
    [SerializeField] Transform gunPoint;
    [SerializeField] Transform subGunPoint;
    [SerializeField] GameObject cannonPrefab;
    [SerializeField] GameObject bulletPrefab;

    [Header("Reload Time")]
    [SerializeField] float mainGunReloadTime;

    private bool isMainGunReady = true;
    private float currentReloadTime;
    public event Action<float> OnReloadProgressChanged;

    // 주포를 발사합니다.
    private void MainGunFire()
    {
        if (isMainGunReady == true)
        {
            isMainGunReady=false;
            GameObject shell = Instantiate(cannonPrefab, gunPoint.position, gunPoint.rotation);
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

        while (currentReloadTime >= 0f)
        {
            currentReloadTime -= Time.deltaTime; // 현재 재장전 진행 상황 업데이트
            if(currentReloadTime < 0f)
            {
                currentReloadTime = 0f;
            }
            OnReloadProgressChanged?.Invoke(1 - (currentReloadTime / reloadTime)); // 이벤트 호출
            yield return null;
        }
        isMainGunReady = true;
    }

    #endregion

    #region InputSystem
    // 주포 공격 키 입력
    private void OnMouseLeft(InputValue key) 
    {
        if(key.isPressed == true)
        {
            MainGunFire();
        }
    }

    // 동축기관총 키 입력
    private void OnSubAttack(InputValue key)
    {
        if(key.isPressed == true)
        {
            SubGunFire();
        }
    }
    #endregion;
}
