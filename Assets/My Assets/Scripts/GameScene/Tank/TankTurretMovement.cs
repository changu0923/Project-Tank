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
    [SerializeField] Transform gunAimTransform;
    [SerializeField] Transform turret;
    [SerializeField] Transform gun;
    [SerializeField] Transform gunPoint;
    [Header("포탑 회전 속도")]
    [SerializeField] float rotationSpeed;

    [Header("주포 내림각 제한")]
    
    [SerializeField] float maxDepression;

    [Header("주포 올림각 제한")]
    [SerializeField] float maxElevation;

    private int playerLayer;
    private bool isTurretLock;
    private float currentAngle;

    public float CurrentAngle { get => currentAngle; }
    public Transform AimTransform { get => aimTransform; } 

    private void Start()
    {
        playerLayer = 1 << LayerMask.NameToLayer("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
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
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
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
        Ray ray = new Ray(gunPoint.position, gunPoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~playerLayer))
        {
            gunAimTransform.position = hit.point;
        }
        else
        {
            
            Vector3 targetPos = ray.origin + ray.direction * 100f;
            gunAimTransform.position = targetPos;
        }


        Vector3 localTargetPos = turret.InverseTransformDirection(aimTransform.position - gun.position);
        Vector3 zeroPlainVector = Vector3.ProjectOnPlane(localTargetPos, Vector3.up);

        float angle = Vector3.Angle(zeroPlainVector, localTargetPos);
        angle *= Mathf.Sign(localTargetPos.y);
        angle = Mathf.Clamp(angle, -maxDepression, maxElevation);

        currentAngle = Mathf.MoveTowards(currentAngle, angle, rotationSpeed * Time.deltaTime);
        if(Mathf.Abs(currentAngle) > Mathf.Epsilon)
        {
            gun.localEulerAngles = Vector3.right * -currentAngle;
        }
    }   

    // 터렛 잠금 기능
    void OnMouseRight(InputValue key)
    {
        isTurretLock = key.isPressed ? true : false;
    }
}    