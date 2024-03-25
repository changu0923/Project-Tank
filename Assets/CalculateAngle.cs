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
        // 1. Ÿ�ٰ� �ڽ��� �Ÿ��� ����(����)
        Vector3 dir = (target.position - transform.position);
        distanceBetween = Vector3.Distance(transform.position, target.position);
        resultDirX = dir.x;
        resultDirY = dir.y;
        resultDirZ = dir.z;

        // 2. B���� A�� +Y�� ��鿡 ���� ������ ���� ���ϱ� (�����ﰢ���� ����)
        Vector3 directionAB = target.position - transform.position;
        resultNormalBtoA = Vector3.Dot(directionAB, Vector3.up);

        // 3. �ظ��� ���� ���ϱ� 
        baseLength = Mathf.Sqrt((distanceBetween * distanceBetween) - (resultNormalBtoA * resultNormalBtoA));
        sb.AppendLine($"hypotenuse : {distanceBetween}");
        sb.AppendLine($"height  : {resultNormalBtoA}");
        sb.AppendLine($"base : {baseLength}");
        debugText.text =  sb.ToString();
    }
}
