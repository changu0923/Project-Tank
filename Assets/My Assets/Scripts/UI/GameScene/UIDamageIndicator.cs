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
    [SerializeField] Image damageArrowImage;

    [Header("Test")]
    public Vector3 damageLocationTest;

    public bool isOn = false;

    private Coroutine destroyCoroutine = null;

    private void Update()
    {
        if (isOn == true)
        {
            damageLocation.y = playerObject.position.y;
            Vector3 dir = (damageLocation - playerObject.transform.position).normalized;
            float angle = (Vector3.SignedAngle(dir, playerObject.forward, Vector3.up));
            DamageImagePivot.transform.localEulerAngles = new Vector3(0, 0, angle);            
            destroyCoroutine ??= StartCoroutine(DestorySelf(3f));            
        }
    }

    public void SetIndicatorInfo(string shotFrom, int damage, Vector3 currentPos, Transform myPositon)
    {
        playerObject = myPositon;
        damageLocationTest = currentPos;
        damageLocation = currentPos;
        damageFromText.text = shotFrom;

        if (damage != 0)
        {
            damageAmountText.text = damage.ToString();
        }
        else
        {
            damageArrowImage.color = Color.black;
            damageAmountText.text = "Blocked";
        }

        isOn = true;    
    }

    private IEnumerator DestorySelf(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
}
