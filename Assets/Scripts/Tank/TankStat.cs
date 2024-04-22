using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TankStat : MonoBehaviour
{
    [SerializeField] string tankName;
    [SerializeField] int maxHP;
    [SerializeField] int maxSpeed;

    [Header("Camo")]
    [SerializeField] List<MeshRenderer> targetRenderers = new List<MeshRenderer>();
    private Material currentCamo;

    public void SetVehicleCamo(Material inputMaterial)
    {
        currentCamo = inputMaterial;
        foreach (var renderer in targetRenderers) 
        { 
            renderer.material = currentCamo;  
        }
    }
}
