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
        aimTransform = GameManager.Instance.targetTransform;
    }

    void FixedUpdate()
    {
        if (rb.velocity != Vector3.zero)    // velcocity�� ���� ȭ�찢����ȭ 
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

        // TODO : ���ư���
        Vector3 direction = (aimTransform.position - transform.position).normalized;
        transform.LookAt(aimTransform);
        rb.AddForce(direction * shellSpeed * 10f, ForceMode.Impulse);
        StartCoroutine(DestroySelf());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") == false)
        {
            if (collision.collider.CompareTag("Armor"))
            {
                Armor targetArmor = collision.collider.GetComponent<Armor>();

                // �浹�� ��ü�� ǥ�� ��� ���� (����ȭ�� ��� ���� ���)
                Vector3 surfaceNormal = collision.contacts[0].normal;

                // �Ѿ��� ���� ���� (�Ѿ��� Ƣ��� ����)
                Vector3 bulletDirection = transform.forward;

                // �Ի簢 ��� (�� ������ ���� ���)
                float incidenceAngle = Vector3.Angle(-bulletDirection, surfaceNormal);

                // ��� ���� �β� ���
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
        // �Ի簢�� 90�� ������ ��쿡�� ����� �����մϴ�.
        if (incidenceAngle <= 90f)
        {
            // �Ի簢�� ������� �Ի簢���� ��ȯ�մϴ�.
            float relativeIncidenceAngle = 90f - incidenceAngle;

            // ���� ���� ����Ͽ� ��� ���� �β��� ����մϴ�.
            return actualThickness / Mathf.Sin(relativeIncidenceAngle * Mathf.Deg2Rad);
        }
        else
        {
            // �Ի簢�� 90���� �ʰ��ϴ� ���, ������� �Ի簢�� 90���� �����մϴ�.
            return actualThickness;
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
