using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHangarPanel : MonoBehaviour
{
    public UITopBarPanel topBarPanel;
    public UIBottomBarPanel bottomBarPanel;
    public UIEquipmentPanel equipmentPanel;

    [Header("Vehicle Viewer")]
    [SerializeField] Transform vehicleSpawnPoint;
    private GameObject currentViewModel;
    private Dictionary<string, GameObject> ownedVehicles = new Dictionary<string, GameObject>();

    private void Awake()
    {
        UIManager.Instance.hangarPanel = this;
    }
    
    public void GetVehicle(string tankName, GameObject tankPrefab)
    {
        ownedVehicles.Add(name, tankPrefab);
    }

    // toggle�� ���õ� ������ 3d���� ���� ǥ���մϴ�.
    public void GetViewModel(string vehicleName)
    {
        if(vehicleSpawnPoint.childCount == 0) 
        {
            currentViewModel = Instantiate(ownedVehicles[vehicleName].gameObject, vehicleSpawnPoint);   
        }
        else
        {
            GameObject currentModel = vehicleSpawnPoint.GetChild(0).gameObject;
            Destroy(currentModel);

            currentViewModel = Instantiate(ownedVehicles[vehicleName].gameObject, vehicleSpawnPoint);
        }
    }
}
