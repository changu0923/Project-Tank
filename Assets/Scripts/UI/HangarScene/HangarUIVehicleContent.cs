using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HangarUIVehicleContent : MonoBehaviour
{
    [SerializeField] Image vehicleIcon;
    [SerializeField] Image vehicleNation;
    [SerializeField] Text tankNameText;

    private Toggle toggle;

    private TankData currentTankData; 
    public TankData GetCurrentTankData { get => currentTankData; }
    public TankData SetCurrentTankData { set => currentTankData = value;  }
    public Toggle Toggle { get => toggle; set => toggle = value; }

    private void Awake()
    {
        Toggle = GetComponent<Toggle>();
        Toggle.group = transform.GetParentComponent<ToggleGroup>();
        Toggle.onValueChanged.AddListener(OnTogglePressed);
    }

    private void OnTogglePressed(bool bValue)
    {
        if(bValue)
        {
            DatabaseManager.Instance.SelectedTank = currentTankData;
            Debug.Log($"Data Set : {DatabaseManager.Instance.SelectedTank.TankName}");
            UIManager.Instance.hangarPanel.ShowSelectedVehicle(DatabaseManager.Instance.SelectedTank.TankName);            
        }
        else
        {
            DatabaseManager.Instance.SelectedTank = null;
            UIManager.Instance.hangarPanel.ClearVehicle();
        }
    }
    public void InitializeTankData()
    {
        tankNameText.text = currentTankData.TankName;
        vehicleNation.sprite = UIManager.Instance.hangarPanel.ModelList.GetSprite(currentTankData.TankNation);
        //TODO : ICON
    }
}
