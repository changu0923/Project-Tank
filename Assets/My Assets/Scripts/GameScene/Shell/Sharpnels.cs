using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sharpnels : MonoBehaviour
{
    [SerializeField] GameObject sharpnelPrefab;
    private List<GameObject> sharpnelList = new List<GameObject>();
    private string shooter;

    public string Shooter { get => shooter;}

    public void SpawnRandomizeSharpnel(string shooterName)
    {
        shooter = shooterName;
        int randomCount = Random.Range(3, 13);
        for(int i = 0; i<randomCount; i++)
        {
            GameObject newSharpnel = Instantiate(sharpnelPrefab, transform.position, Quaternion.identity);
            newSharpnel.name = shooterName;
            sharpnelList.Add(newSharpnel);
        }
        StartExplosion();
    }

    public void StartExplosion()
    {
        Vector3 circlePos = transform.position + transform.forward * 2f;

        foreach (var sharpnel in sharpnelList)
        {
            Vector2 randomPointInCircle = Random.insideUnitCircle * 2f;
            Vector3 shrapnelTargetPos = circlePos + new Vector3(randomPointInCircle.x, 0f, randomPointInCircle.y);
            Vector3 dir = (shrapnelTargetPos - sharpnel.transform.position).normalized;
            Rigidbody rb = sharpnel.GetComponent<Rigidbody>();            
            if(rb != null)
            {
                rb.AddForce(dir * 100f, ForceMode.Impulse);
            }
        }
        StartCoroutine(DestorySharpnels());
    }

    IEnumerator DestorySharpnels()
    {
        yield return new WaitForSeconds(.1f);
        foreach (var sharp in sharpnelList)
        {
            Destroy(sharp);
        }
        Destroy(gameObject);
    }
}
