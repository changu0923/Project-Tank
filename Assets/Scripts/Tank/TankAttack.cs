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

    // ������ �߻��մϴ�.
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

        while (currentReloadTime >= 0f)
        {
            currentReloadTime -= Time.deltaTime; // ���� ������ ���� ��Ȳ ������Ʈ
            if(currentReloadTime < 0f)
            {
                currentReloadTime = 0f;
            }
            OnReloadProgressChanged?.Invoke(1 - (currentReloadTime / reloadTime)); // �̺�Ʈ ȣ��
            yield return null;
        }
        isMainGunReady = true;
    }

    #endregion

    #region InputSystem
    // ���� ���� Ű �Է�
    private void OnMouseLeft(InputValue key) 
    {
        if(key.isPressed == true)
        {
            MainGunFire();
        }
    }

    // �������� Ű �Է�
    private void OnSubAttack(InputValue key)
    {
        if(key.isPressed == true)
        {
            SubGunFire();
        }
    }
    #endregion;
}
