using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBottomBarPanel : MonoBehaviour
{
    [SerializeField] RectTransform vehicleHolder;
    [SerializeField] GameObject contentPrefab;

    public RectTransform GetVehicleHolder { get => vehicleHolder;}

    private void Awake()
    {
        UpdateVehicleElements();
    }

    private void UpdateVehicleElements()
    {
        foreach (TankData element in DatabaseManager.Instance.CurrentUserOwnedVehicles) 
        {
            HangarUIVehicleContent content = Instantiate(contentPrefab, vehicleHolder).GetComponent<HangarUIVehicleContent>();
            content.SetCurrentTankData = element;
            content.InitializeTankData();
        }
    }
}
