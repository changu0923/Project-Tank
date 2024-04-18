using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HangarUIVehicleContent : MonoBehaviour
{
    [SerializeField] Image vehicleIcon;
    [SerializeField] Image vehicleNation;
    [SerializeField] Text tankNameText;

    Toggle toggle;

    public Image GetVehicleIcon { get => vehicleIcon; }
    public Image SetVehicleIcon { set => vehicleIcon = value; }
    public Image GetVehicleNation { get => vehicleNation; }
    public Image SetVehicleNation { set => vehicleNation = value; }
    public Text GetTankNameText { get => tankNameText; }
    public Text SetTankNameText { set => tankNameText = value; }

    private void Awake()
    {
    }


}
