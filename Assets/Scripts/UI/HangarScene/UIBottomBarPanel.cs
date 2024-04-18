using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBottomBarPanel : MonoBehaviour
{
    [SerializeField] RectTransform VehicleHolder;
    [SerializeField] GameObject contentPrefab;

    private void Awake()
    {
        UpdateVehicleElements();
    }

    private void UpdateVehicleElements()
    {
        foreach (TankData element in DatabaseManager.Instance.CurrentUserOwnedVehicles) 
        {
            HangarUIVehicleContent content = Instantiate(contentPrefab, VehicleHolder).GetComponent<HangarUIVehicleContent>();
            content.GetTankNameText.text = element.TankName;
            content.GetVehicleNation.sprite = UIManager.Instance.hangarPanel.ModelList.GetSprite(element.TankNation);
        }
    }
}
