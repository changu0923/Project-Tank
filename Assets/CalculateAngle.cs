using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Android.Types;
using UnityEngine;

public class CalculateAngle : MonoBehaviour
{
    [SerializeField] Transform target;
    Vector3 resultAngles;
    StringBuilder sb = new StringBuilder();
    [SerializeField] TextMeshProUGUI debugText; 

    float distanceBetween;
    float resultDirX;
    float resultDirY;
    float resultDirZ;
    float resultNormalBtoA;
    float baseLength;

    private void Update()
    {
        sb.Clear();
        // 1. 타겟과 자신의 거리를 구함(빗변)
        Vector3 dir = (target.position - transform.position);
        distanceBetween = Vector3.Distance(transform.position, target.position);
        resultDirX = dir.x;
        resultDirY = dir.y;
        resultDirZ = dir.z;

        // 2. B에서 A의 +Y축 평면에 대해 법선의 길이 구하기 (직각삼각형의 높이)
        Vector3 directionAB = target.position - transform.position;
        resultNormalBtoA = Vector3.Dot(directionAB, Vector3.up);

        // 3. 밑면의 길이 구하기 
        baseLength = Mathf.Sqrt((distanceBetween * distanceBetween) - (resultNormalBtoA * resultNormalBtoA));
        sb.AppendLine($"hypotenuse : {distanceBetween}");
        sb.AppendLine($"height  : {resultNormalBtoA}");
        sb.AppendLine($"base : {baseLength}");
        debugText.text =  sb.ToString();
    }
}
