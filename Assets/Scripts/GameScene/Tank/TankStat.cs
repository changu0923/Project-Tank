using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TankStat : MonoBehaviour
{
    [SerializeField] string tankName;
    [SerializeField] int maxHP;
    [SerializeField] int maxSpeed;
    private int currentHP;
    private int currentSpeed;
    private bool isDestoryed;

    [Header("Camo")]
    [SerializeField] List<MeshRenderer> targetRenderers = new List<MeshRenderer>();
    private Material currentCamo;
    private Material destroyedMaterial;

    private void Awake()
    {
        destroyedMaterial = Resources.Load<Material>("MaterialDestroyed"); ;

        currentHP = maxHP;
        isDestoryed = false;
    }

    public void SetVehicleCamo(Material inputMaterial)
    {
        currentCamo = inputMaterial;
        foreach (var renderer in targetRenderers) 
        { 
            renderer.material = currentCamo;  
        }

        Debug.Log($"SetVehicleCamo() Called : Input name : {inputMaterial.name}. Current Set : {currentCamo.name}");
    }

    public void TakeDamage(int damage)
    {
        if(!isDestoryed) 
        {
            currentHP -= damage;
            print($"{transform.name} take Damage : [{damage}], Current HP is : {currentHP}");
            if(currentHP <= 0)
            {
                isDestoryed = true;
                TankDestroyed();
            }
        }
    }

    private void TankDestroyed()
    {        
        SetVehicleCamo(destroyedMaterial);
    }
}
