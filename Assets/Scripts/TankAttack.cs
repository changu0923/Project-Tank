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

    // ������ �߻��մϴ�.
    private void MainGunFire()
    {

    }

    // ���������� �߻��մϴ�.
    private void SubGunFire()
    {

    }

    #region InputSystem
    // ���� ���� Ű �Է�
    private void OnAttack(InputValue key) 
    {
        
    }

    // �������� Ű �Է�
    private void OnSubAttack(InputValue key)
    {

    }
    #endregion;
}
