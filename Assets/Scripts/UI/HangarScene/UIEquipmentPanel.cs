using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentPanel : MonoBehaviour
{
    [Header("Camo Holder")]
    [SerializeField] Button camoButton;
    [SerializeField] ScrollRect camoContentHolder;
    [SerializeField] VerticalLayoutGroup camoContent;
    [SerializeField] List<Toggle> camoToggles = new List<Toggle>(); 
    private bool isCamoButtonOn;
    private void Awake()
    {
        camoButton.image.sprite = camoToggles[0].GetComponentInChildren<Image>().sprite;
        camoContentHolder.gameObject.SetActive(false);
        camoButton.onClick.AddListener(OnCamoButtonClick);
        foreach (Toggle toggle in camoToggles) 
        {
            toggle.onValueChanged.AddListener(OnCamoToggleOn);
        }
    }
    private void OnCamoButtonClick()
    {
        isCamoButtonOn = !isCamoButtonOn;
        camoContentHolder.gameObject.SetActive(isCamoButtonOn);
    }

    private void OnCamoToggleOn(bool bValue)
    {
        int index = 0;
        foreach (Toggle toggle in camoToggles) 
        {
            if(toggle.isOn) 
            {
                Sprite newSprite = toggle.GetComponentInChildren<Image>().sprite;
                Image buttonImage = camoButton.GetComponent<Image>();
                buttonImage.sprite = newSprite;

                TankStat targetTank;
                bool result = UIManager.Instance.hangarPanel.VehicleSpawnPoint.GetChild(0).TryGetComponent<TankStat>(out targetTank);
                if(result)
                {
                    targetTank.SetVehicleCamo(UIManager.Instance.hangarPanel.ModelList.CamoMaterials[index]);
                }
                DatabaseManager.Instance.ChangeCamo(index);
                break;
            }
            index++;
        }
    }


}

