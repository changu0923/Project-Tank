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
        GameObject shell = Instantiate(cannonPrefab, gunPoint.position, gunPoint.rotation);
        shell.GetComponent<Shell>().Fire();
    }

    // ���������� �߻��մϴ�.
    private void SubGunFire()
    {

    }

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

    }
    #endregion;
}
