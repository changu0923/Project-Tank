using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHangarPanel : MonoBehaviour
{
    public UITopBarPanel topBarPanel;
    public UIBottomBarPanel bottomBarPanel;
    public UIEquipmentPanel equipmentPanel;

    [Header("Vehicle Viewer")]
    [SerializeField] ModelList modelList;
    [SerializeField] Transform vehicleSpawnPoint;

    public Transform VehicleSpawnPoint { get => vehicleSpawnPoint; set => vehicleSpawnPoint = value; }
    public ModelList ModelList { get => modelList; set => modelList = value; }

    private void Awake()
    {
        UIManager.Instance.hangarPanel = this;
    } 
}
