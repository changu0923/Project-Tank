using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor.Build.Content;
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
            Debug.Log($"OnTogglePressed : {DateTime.Now}");
            DatabaseManager.Instance.SelectedTank = currentTankData;
            UIManager.Instance.hangarPanel.ShowSelectedVehicle(tankNameText.text);
        }
        else
        {
            UIManager.Instance.hangarPanel.ClearVehicle();
            DatabaseManager.Instance.SelectedTank = null;
        }
    }

    public void InitializeTankData()
    {
        tankNameText.text = currentTankData.TankName;
        vehicleNation.sprite = UIManager.Instance.hangarPanel.ModelList.GetSprite(currentTankData.TankNation);
        //TODO : ICON
    }
}
