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
        if (rb.velocity != Vector3.zero)    // velcocity에 따른 화살각도변화 
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

        // TODO : 날아가기
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

                // 충돌한 객체의 표면 노멀 벡터 (정규화된 노멀 벡터 사용)
                Vector3 surfaceNormal = collision.contacts[0].normal;

                // 총알의 방향 벡터 (총알이 튀어나온 방향)
                Vector3 bulletDirection = transform.forward;

                // 입사각 계산 (두 벡터의 각도 계산)
                float incidenceAngle = Vector3.Angle(-bulletDirection, surfaceNormal);

                // 상대 갑옷 두께 계산
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
        // 입사각이 90도 이하인 경우에만 계산을 수행합니다.
        if (incidenceAngle <= 90f)
        {
            // 입사각을 상대적인 입사각으로 변환합니다.
            float relativeIncidenceAngle = 90f - incidenceAngle;

            // 사인 값을 사용하여 상대 갑옷 두께를 계산합니다.
            return actualThickness / Mathf.Sin(relativeIncidenceAngle * Mathf.Deg2Rad);
        }
        else
        {
            // 입사각이 90도를 초과하는 경우, 상대적인 입사각은 90도로 설정합니다.
            return actualThickness;
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
