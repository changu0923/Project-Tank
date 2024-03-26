using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
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

    public float currentAngle;

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
        Vector3 localTargetPos = turret.InverseTransformDirection(aimTransform.position - gun.position);
        Vector3 zeroPlainVector = Vector3.ProjectOnPlane(localTargetPos, Vector3.up);
        float angle = Vector3.Angle(zeroPlainVector, localTargetPos);
        angle *= Mathf.Sign(localTargetPos.y);
        currentAngle = Mathf.MoveTowards(currentAngle, angle, rotationSpeed * Time.deltaTime);
        if(Mathf.Abs(currentAngle) >Mathf.Epsilon)
        {
            gun.localEulerAngles = Vector3.right * - currentAngle;
        }
    }   

    // 터렛 잠금 기능
    void OnMouseRight(InputValue key)
    {
        isTurretLock = key.isPressed ? true : false;
    }
}
    