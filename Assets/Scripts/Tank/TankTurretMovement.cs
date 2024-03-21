using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankTurretMovement : MonoBehaviour
{
    [SerializeField] Transform aimTransform;
    [SerializeField] Transform turret;
    [SerializeField] Transform gun;
    [Header("포탑 회전 속도")]
    [SerializeField] float rotationSpeed;

    [Header("주포 내림각 제한")]
    [SerializeField] float minGunAngle;

    [Header("주포 올림각 제한")]
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
        if (!isTurretLock)
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

        Vector3 aimDirection = turret.parent.InverseTransformDirection(aimTransform.position - turret.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
        Vector3 targetEulerAngles = targetRotation.eulerAngles;
        targetEulerAngles.x = 0f;
        targetEulerAngles.z = 0f;
        targetRotation = Quaternion.Euler(targetEulerAngles);
        turret.localRotation = Quaternion.RotateTowards(turret.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void GunMove()
    {
        // aimTransform과 gun 사이의 방향을 구함
        Vector3 aimDirection = (aimTransform.position - gun.position).normalized;

        // aimDirection을 gun의 로컬 좌표계로 변환
        Vector3 localAimDirection = gun.parent.InverseTransformDirection(aimDirection);

        // 로컬 방향 벡터를 각도로 변환
        float targetAngleX = Mathf.Atan2(localAimDirection.y, localAimDirection.z) * Mathf.Rad2Deg;

        // aimTransform이 gun 아래에 있으면 위쪽을 조준하고, 위에 있으면 아래쪽을 조준
        if (aimTransform.position.y < gun.position.y)
        {
            targetAngleX = Mathf.Max(targetAngleX, minGunAngle);
        }
        else
        {
            targetAngleX = Mathf.Min(targetAngleX, maxGunAngle);
        }

        // x축 각도만 사용하여 위아래로 조준
        Quaternion targetRotation = Quaternion.Euler(targetAngleX, 0f, 0f);

        // 현재 각도에서 목표 각도로 부드럽게 회전
        gun.localRotation = Quaternion.RotateTowards(gun.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    // 터렛 잠금 기능
    void OnMouseRight(InputValue key)
    {
        isTurretLock = key.isPressed ? true : false;
    }
}
    