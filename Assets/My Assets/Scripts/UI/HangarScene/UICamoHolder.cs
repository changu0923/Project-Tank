using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    private void OnEnable()
    {
        UIManager.Instance.hangarPanel.loadingPanel.gameObject.SetActive(true);
        selectedTankData = DatabaseManager.Instance.SelectedTank;
        StartCoroutine(SetCamoCoroutine(selectedTankData.CamoSlot));     
    }

    
    public void SetCamoOnUI(int index)
    {
        Image currentCamoImage = camoButton.GetComponent<Image>();
        Sprite targetSprite = UIManager.Instance.hangarPanel.ModelList.CamoSprites[index];
        currentCamoImage.sprite = targetSprite;
        camoToggles[index].isOn = true;
    }

    public void SetCamoOnVehicle(int index)
    {
        TankStat currentVehicle;
        if (UIManager.Instance.hangarPanel.VehicleSpawnPoint.GetChild(0).TryGetComponent<TankStat>(out currentVehicle))
        {            
            Material targetMaterial = UIManager.Instance.hangarPanel.ModelList.CamoMaterials[index];
            currentVehicle.SetVehicleCamo(targetMaterial);
        }
    }
    IEnumerator SetCamoCoroutine(int index)
    {
        yield return new WaitForSeconds(0.5f);
        SetCamoOnUI(index);
        SetCamoOnVehicle(index);
        UIManager.Instance.hangarPanel.loadingPanel.gameObject.SetActive(false); 
    }
}
    

