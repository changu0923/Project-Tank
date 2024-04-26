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

    // 이 UI가 OnEnable 되는 경우는 Bottom Bar에 있는 차량을 선택했을 경우이다.
    private void OnEnable()
    {
        // 1. Bottom Bar에서 선택과 동시에 DatabaseManager에 저장한 Selected Tank의 데이터를 가져옴
        selectedTankData = DatabaseManager.Instance.SelectedTank;
        Debug.Log($"Data Get : {selectedTankData.TankName}");
        int camoIndex = selectedTankData.CamoSlot;
        if (selectedTankData != null)
        {
            // 2. selected Tank의 스프라이트 가져와서 UI에 보여주기
            Image currentCamoImage = camoButton.GetComponent<Image>();
            Sprite targetSprite = UIManager.Instance.hangarPanel.ModelList.CamoSprites[camoIndex];
            currentCamoImage.sprite = targetSprite;

            // 3. toggleList에 있는 camo 들 중 selectedTankData에 있는 camo를 적용시킴
            camoToggles[camoIndex].isOn = true;

            // 4. UI의 적용이 끝났으면 실제 모델에도 camo 적용
            SetCamoOnVehicle(camoIndex);
        }
    }

    // 실제로 Hangar에 보여지는 차량에 camo 적용
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
    

