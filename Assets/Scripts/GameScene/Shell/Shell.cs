using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] protected string shellName;
    [SerializeField] protected float shellPenetration;
    [SerializeField] protected float shellSpeed;
    [SerializeField] protected int shellDamage;

    public Transform aimTransform;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        aimTransform = TestGameManager.Instance.targetTransform;
    }

    void FixedUpdate()
    {
        // ��ź ������ ����
        if (rb.velocity != Vector3.zero)    // velcocity�� ���� ź�� ���� ��ȭ 
        {
            rb.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    public virtual void Fire()
    {
        if(aimTransform == null)
        {
            print("aimTransform is null");
            return;
        }
       Vector3 direction = transform.forward;
        rb.AddForce(direction * shellSpeed * 1f, ForceMode.Impulse);
        StartCoroutine(DestroySelf());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") == false)
        {
            if (collision.collider.CompareTag("Armor"))
            {
                print($"Collision Detected : [{collision.collider.name}] : [{DateTime.Now}]");
                Armor targetArmor = collision.collider.GetComponent<Armor>();

                // �浹�� ��ü�� ǥ�� ��� ���� (����ȭ�� ��� ���� ���)
                Vector3 surfaceNormal = collision.contacts[0].normal;

                // �Ѿ��� ���� ���� (�Ѿ��� Ƣ��� ����)
                Vector3 bulletDirection = transform.forward;

                // �Ի簢 ��� (�� ������ ���� ���)
                float incidenceAngle = Vector3.Angle(-bulletDirection, surfaceNormal);

                // �尩���
                float relativeThickness = CalculateRelativeThickness(targetArmor.GetArmorThickness, incidenceAngle);

                if (relativeThickness < shellPenetration)
                {
                    print("Penetration Success : " + relativeThickness + "mm");
                    targetArmor.Penetrated();
                    Destroy(gameObject);
                }
                else
                {
                    print("Penetration Failed : " + relativeThickness + "mm");
                }
            }
        }
    }

    private float CalculateRelativeThickness(float actualThickness, float incidenceAngle)
    {
        // �Ի簢�� 90�� ������ ��쿡�� ���
        if (incidenceAngle <= 90f)
        {
            // �Ի簢�� ������� �Ի簢���� ��ȯ
            float relativeIncidenceAngle = 90f - incidenceAngle;

            // ���� ���� ����Ͽ� ��� ���� �β��� ���
            return actualThickness / Mathf.Sin(relativeIncidenceAngle * Mathf.Deg2Rad);
        }
        else
        {
            // �Ի簢�� 90���� �ʰ��ϴ� ���, ������� �Ի簢�� 90���� ����
            return actualThickness;
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}