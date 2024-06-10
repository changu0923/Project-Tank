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
    [Header("��ž ȸ�� �ӵ�")]
    [SerializeField] float rotationSpeed;

    [Header("���� ������ ����")]
    
    [SerializeField] float maxDepression;

    [Header("���� �ø��� ����")]
    [SerializeField] float maxElevation;

    [Header("�ͷ� ������ҽ�")]
    [SerializeField] AudioSource audioSource;

    private int playerLayer;
    private bool isTurretLock;
    private float currentAngle;
    private Quaternion previousRotation;
    private float turretMoveTimer = 0f;

    public float CurrentAngle { get => currentAngle; }
    public Transform AimTransform { get => aimTransform; }
    public Transform GunAimTransform { get => gunAimTransform; }

    private void Start()
    {
        previousRotation = turret.localRotation;
        audioSource.clip = AudioManager.Instance.turretSound;
        audioSource.loop = true;
        playerLayer = 1 << LayerMask.NameToLayer("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        TurretMove();
        GunMove();
        HandleTurretSound();
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

        Vector3 zeroPlainWorldStart = gun.position;
        Vector3 zeroPlainWorldEnd = turret.TransformDirection(zeroPlainVector) + gun.position;

        Debug.DrawLine(zeroPlainWorldStart, zeroPlainWorldEnd, Color.green);
        Debug.DrawLine(aimTransform.position, zeroPlainWorldEnd, Color.blue);
        Debug.DrawLine(gun.position, aimTransform.position, Color.red);

        float angle = Vector3.Angle(zeroPlainVector, localTargetPos);
        angle *= Mathf.Sign(localTargetPos.y);
        angle = Mathf.Clamp(angle, -maxDepression, maxElevation);

        currentAngle = Mathf.MoveTowards(currentAngle, angle, rotationSpeed * Time.deltaTime);
        if(Mathf.Abs(currentAngle) > Mathf.Epsilon)
        {
            gun.localEulerAngles = Vector3.right * -currentAngle;
        }
    }

    private void HandleTurretSound()
    {
        float turretStillTime = 0.15f;
        if (turret.localRotation != previousRotation)
        {
            turretMoveTimer = 0f;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            turretMoveTimer += Time.deltaTime;  

            if (turretMoveTimer >= turretStillTime && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        previousRotation = turret.localRotation;
    }

    // �ͷ� ��� ���
    void OnMouseRight(InputValue key)
    {
        isTurretLock = key.isPressed ? true : false;
    }
}    