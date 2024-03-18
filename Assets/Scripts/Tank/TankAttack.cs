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

    // 주포를 발사합니다.
    private void MainGunFire()
    {

    }

    // 동축기관총을 발사합니다.
    private void SubGunFire()
    {

    }

    #region InputSystem
    // 주포 공격 키 입력
    private void OnAttack(InputValue key) 
    {
        
    }

    // 동축기관총 키 입력
    private void OnSubAttack(InputValue key)
    {

    }
    #endregion;
}
