using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDamageIndicator : MonoBehaviour
{
    public Vector3 damageLocation;
    public Transform playerObject;
    public Transform DamageImagePivot;

    private void Update()
    {
        damageLocation.y= playerObject.position.y;
        Vector3 dir = (damageLocation - playerObject.transform.position).normalized;
        float angle = (Vector3.SignedAngle(dir, playerObject.forward, Vector3.up));
        DamageImagePivot.transform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
