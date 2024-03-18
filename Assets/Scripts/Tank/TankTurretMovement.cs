using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankTurretMovement : MonoBehaviour
{
    [SerializeField] Transform aimTransform;
    [SerializeField] Transform turret;
    [SerializeField] Transform gun;
    [Header("��ž ȸ�� �ӵ�")]
    [SerializeField] float rotationSpeed;

    [Header("���� ������ ����")]
    [SerializeField] float minGunAngle;

    [Header("���� �ø��� ����")]
    [SerializeField] float maxGunAngle;
    private int playerLayer;

    private bool isTurretLock;

    private void Start()
    {
        playerLayer = 1 << LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        TurretMove();
        GunMove();
    }

    public void TurretMove()
    {
        if (isTurretLock==false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~playerLayer))
            {
                Vector3 targetPos = hit.point;
                aimTransform.position = targetPos;
            }
            else
            {
                Vector3 targetPos = ray.origin + ray.direction * 100f;
                aimTransform.position = targetPos;
            }
        }
            Vector3 aimDirection = (aimTransform.position - turret.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
            targetRotation.x = 0f;
            targetRotation.z = 0f;
            turret.rotation = Quaternion.RotateTowards(turret.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        
    }
    public void GunMove()
    {
        Vector3 aimDirection = (aimTransform.position - gun.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
        targetRotation.eulerAngles = new Vector3(targetRotation.eulerAngles.x, gun.rotation.eulerAngles.y, 0f);
        gun.rotation = Quaternion.RotateTowards(gun.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    // �ͷ� ��� ���
    void OnMouseRight(InputValue key)
    {
        print(key.isPressed);
        isTurretLock = key.isPressed ? true : false;
    }
}
    