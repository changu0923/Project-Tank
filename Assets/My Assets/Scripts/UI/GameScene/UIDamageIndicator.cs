using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDamageIndicator : MonoBehaviour
{
    private Vector3 damageLocation;
    private Transform playerObject;
    [SerializeField] Transform DamageImagePivot;
    [SerializeField] Text damageFromText;
    [SerializeField] Text damageAmountText;

    private bool isOn = false;

    private Coroutine destroyCoroutine = null;

    private void Update()
    {
        if (isOn == true)
        {
            damageLocation.y = playerObject.position.y;
            Vector3 dir = (damageLocation - playerObject.transform.position).normalized;
            float angle = (Vector3.SignedAngle(dir, playerObject.forward, Vector3.up));
            DamageImagePivot.transform.localEulerAngles = new Vector3(0, 0, angle);            
            destroyCoroutine ??= StartCoroutine(DestorySelf(2f));            
        }
    }

    public void SetIndicatorInfo(string shotFrom, int damage, Vector3 currentPos)
    {
        print($"SetIndicatorInfo Called [UIDamageIndicator]: {shotFrom},{damage},{currentPos}");
        damageLocation = currentPos;
        damageFromText.text = shotFrom;
        damageAmountText.text = damage.ToString();
        isOn = true;    
    }

    private IEnumerator DestorySelf(float t)
    {
        print("DestorySelf Called");
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
}
