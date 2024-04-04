using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReticleTPS : MonoBehaviour
{ 
    [SerializeField]RectTransform inCircleReticle;
    [SerializeField]RectTransform outCircleReticle;
    [SerializeField]Transform aimTrasform;
    [SerializeField]Transform gunAimTrasform;
    void Update()
    {
        Vector3 screenMousePosition = Camera.main.WorldToScreenPoint(aimTrasform.position);
        Vector3 screenGunAimPosition = Camera.main.WorldToScreenPoint(gunAimTrasform.position);

        inCircleReticle.position = screenGunAimPosition;
        outCircleReticle.position = screenMousePosition;
    }
}
