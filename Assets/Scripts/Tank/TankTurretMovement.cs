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
        // aimTransform�� gun ������ ������ ����
        Vector3 aimDirection = (aimTransform.position - gun.position).normalized;

        // aimDirection�� gun�� ���� ��ǥ��� ��ȯ
        Vector3 localAimDirection = gun.parent.InverseTransformDirection(aimDirection);

        // ���� ���� ���͸� ������ ��ȯ
        float targetAngleX = Mathf.Atan2(localAimDirection.y, localAimDirection.z) * Mathf.Rad2Deg;

        // aimTransform�� gun �Ʒ��� ������ ������ �����ϰ�, ���� ������ �Ʒ����� ����
        if (aimTransform.position.y < gun.position.y)
        {
            targetAngleX = Mathf.Max(targetAngleX, minGunAngle);
        }
        else
        {
            targetAngleX = Mathf.Min(targetAngleX, maxGunAngle);
        }

        // x�� ������ ����Ͽ� ���Ʒ��� ����
        Quaternion targetRotation = Quaternion.Euler(targetAngleX, 0f, 0f);

        // ���� �������� ��ǥ ������ �ε巴�� ȸ��
        gun.localRotation = Quaternion.RotateTowards(gun.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    // �ͷ� ��� ���
    void OnMouseRight(InputValue key)
    {
        isTurretLock = key.isPressed ? true : false;
    }
}
    