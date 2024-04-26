using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICamoHolder : MonoBehaviour
{
    [Header("Camo Holder")]
    [SerializeField] Button camoButton;
    [SerializeField] ScrollRect camoContentHolder;
    [SerializeField] VerticalLayoutGroup camoContent;
    [SerializeField] List<Toggle> camoToggles = new List<Toggle>();
    
    private bool isCamoButtonOn;
    private TankData selectedTankData;

    // �� UI�� OnEnable �Ǵ� ���� Bottom Bar�� �ִ� ������ �������� ����̴�.
    private void OnEnable()
    {
        // 1. Bottom Bar���� ���ð� ���ÿ� DatabaseManager�� ������ Selected Tank�� �����͸� ������
        selectedTankData = DatabaseManager.Instance.SelectedTank;
        Debug.Log($"Data Get : {selectedTankData.TankName}");
        int camoIndex = selectedTankData.CamoSlot;
        if (selectedTankData != null)
        {
            // 2. selected Tank�� ��������Ʈ �����ͼ� UI�� �����ֱ�
            Image currentCamoImage = camoButton.GetComponent<Image>();
            Sprite targetSprite = UIManager.Instance.hangarPanel.ModelList.CamoSprites[camoIndex];
            currentCamoImage.sprite = targetSprite;

            // 3. toggleList�� �ִ� camo �� �� selectedTankData�� �ִ� camo�� �����Ŵ
            camoToggles[camoIndex].isOn = true;

            // 4. UI�� ������ �������� ���� �𵨿��� camo ����
            SetCamoOnVehicle(camoIndex);
        }
    }

    // ������ Hangar�� �������� ������ camo ����
    public void SetCamoOnVehicle(int index)
    {
        TankStat currentVehicle;
        if (UIManager.Instance.hangarPanel.VehicleSpawnPoint.GetChild(0).TryGetComponent<TankStat>(out currentVehicle))
        {
            Debug.Log($"SetCamoOnVehicle() Called : Vehicle :{currentVehicle.transform.name} Index is {index} Material Should be {UIManager.Instance.hangarPanel.ModelList.CamoMaterials[index].name}");
            Material targetMaterial = UIManager.Instance.hangarPanel.ModelList.CamoMaterials[index];
            currentVehicle.SetVehicleCamo(targetMaterial);
        }
        else 
        {
            Debug.LogError("Load Failed");
        }
    }
}
    

