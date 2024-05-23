using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] protected string shellName;
    [SerializeField] protected float shellPenetration;
    [SerializeField] protected float shellSpeed;
    [SerializeField] protected int shellDamage;
 
    private TrailRenderer trailRenderer;
    private Transform aimTransform;
    private Rigidbody rb;

    private int hitCount = 1;

    // Shooter Information
    private string shooterName;
    private Vector3 shooterPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();        
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

    public void SetAimTransform(Transform target)
    {
        aimTransform = target;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Armor"))
        {      
            if(hitCount == 0) 
            {
                return;
            }

            hitCount--;

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
                targetArmor.Penetrated(GetRandomDamage(), shooterName, shooterPosition);
                OnImpact();
                Destroy(gameObject);
            }
            else
            {
                targetArmor.NotPenetrated(GetRandomDamage(), shooterName, shooterPosition);
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

    private int GetRandomDamage()
    {
        float getPercentage = UnityEngine.Random.Range(-0.15f, 0.15f);
        float multiplier = 1f + getPercentage;
        int finalDamage = Mathf.RoundToInt(shellDamage * multiplier);

        return finalDamage;
    }

    public void SetShooterInfo(string from, Vector3 pos)
    {
        shooterName = from;
        shooterPosition = pos;
    }

    private void OnImpact()
    {
        GameObject trailEndPoint = GameObject.Find("TrailEndPoint");
        if (trailEndPoint != null)
        {
            trailRenderer.transform.SetParent(trailEndPoint.transform);
        }
        else
        {
            trailEndPoint = new GameObject("TrailEndPoint");
            trailEndPoint.transform.position = transform.position;
            trailEndPoint.transform.rotation = transform.rotation;
            trailRenderer.transform.SetParent(trailEndPoint.transform);
        }
        Destroy(trailEndPoint, 0.15f);
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
