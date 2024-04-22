using Photon.Pun;
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

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.group = transform.GetParentComponent<ToggleGroup>();
        toggle.onValueChanged.AddListener(OnTogglePressed);
    }

    private void OnTogglePressed(bool bValue)
    {
        if(bValue)
        {
            UIManager.Instance.hangarPanel.ShowSelectedVehicle(tankNameText.text);
        }
        else
        {
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
