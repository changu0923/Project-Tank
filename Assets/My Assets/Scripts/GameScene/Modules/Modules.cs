using System.Reflection;
using UnityEngine;

public class Modules : MonoBehaviour
{
    [SerializeField] protected int currentHP;
    [SerializeField] protected int maxHP;
    [SerializeField] TankStat tankStat;

    private bool isDestroyed = false;

    private void Start()
    {
        currentHP = maxHP;
    }

    private void TakeDamage(int damage, string shooter)
    {
        if (!isDestroyed)
        {
            currentHP -= damage;
            if(currentHP <= 0)
            {
                currentHP = 0;
                isDestroyed = true;
                tankStat.ModuleAmmoRackDestroyed(shooter);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string shooter = other.gameObject.name;
        Debug.Log( $"Module : {shooter}");
        if (other.gameObject.CompareTag("Sharpnel") == true)
        {
            Destroy(other.gameObject);
            if (isDestroyed == false)
            {
                int damage = Random.Range(50, 501);
                TakeDamage(damage, shooter);
                Debug.Log($"Module TakeDamage : {damage}");
            }
        }
    }
}
