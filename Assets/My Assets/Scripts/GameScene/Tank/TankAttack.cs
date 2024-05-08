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

    [Header("VFX")]
    [SerializeField] GameObject cannonFirePrefab;

    // ��ũ��Ʈ ����� bool ����
    private bool scriptOn;

    private bool isMainGunReady = true;
    private float currentReloadTime;
    private Transform aimTransfrom;

    public bool ScriptOn { get => scriptOn; set => scriptOn = value; }

    // ������ �߻��մϴ�.
    private void MainGunFire()
    {
        if (isMainGunReady == true)
        {
            isMainGunReady=false;
            print($"MainGunFire() From TankAttack.cs Called : [{DateTime.Now}]");
            GameObject shell = Instantiate(cannonPrefab, gunPoint.position, gunPoint.rotation);
            if(aimTransfrom == null )
            {
                aimTransfrom = GetComponent<TankTurretMovement>().AimTransform;
            }
            shell.GetComponent<Shell>().SetAimTransform(aimTransfrom);
            GameObject vfx = Instantiate(cannonFirePrefab, gunPoint.position, gunPoint.rotation);
            Destroy(vfx, 2.5f);
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

    #endregion

    #region InputSystem
    // ���� ���� Ű �Է�
    private void OnMouseLeft(InputValue key) 
    {
        if (scriptOn == false) return;

        if(key.isPressed == true)
        {
            MainGunFire();
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
