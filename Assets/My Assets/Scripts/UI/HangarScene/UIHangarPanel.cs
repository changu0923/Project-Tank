using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHangarPanel : MonoBehaviour
{
    public UITopBarPanel topBarPanel;
    public UIBottomBarPanel bottomBarPanel;
    public UIUserEquipmentPanel equipmentPanel;
    public UIMatchMakingPanel matchmakingPanel;
    public RectTransform loadingPanel;

    [Header("Vehicle Viewer")]
    [SerializeField] ModelList modelList;
    [SerializeField] Transform vehicleSpawnPoint;

    public Transform VehicleSpawnPoint { get => vehicleSpawnPoint; set => vehicleSpawnPoint = value; }
    public ModelList ModelList { get => modelList; set => modelList = value; }

    private void Awake()
    {
        UIManager.Instance.hangarPanel = this;
    }

    private void OnEnable()
    {
        Cursor.visible = true;
    }

    public void ShowSelectedVehicle(string name)
    {
        ClearVehicle();
        GameObject selectedVehicle = modelList.VehicleModelList.Find(prefab => prefab.name == name);  
        if (selectedVehicle != null)
        {
            GameObject currentSpawnVehicle = Instantiate(selectedVehicle, vehicleSpawnPoint.position, Quaternion.Euler(0f, -150f, 0f), vehicleSpawnPoint);
            UIManager.Instance.hangarPanel.equipmentPanel.gameObject.SetActive(true);
        }
    }

    public void ClearVehicle()
    {
        if(vehicleSpawnPoint.childCount > 0) 
        {
            foreach (Transform child in vehicleSpawnPoint)
            {
                Destroy(child.gameObject);
            }            
        }
        UIManager.Instance.hangarPanel.equipmentPanel.gameObject.SetActive(false);
    }
}
