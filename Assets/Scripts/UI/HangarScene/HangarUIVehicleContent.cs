using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HangarUIVehicleContent : MonoBehaviour
{
    [SerializeField] Image vehicleIcon;

    public Image SetVehicleIcon { set => vehicleIcon = value; }
    public Image GetVehicleIcon { get => vehicleIcon;}
}
