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

    [Header("TankComponents")]
    [SerializeField] TankHullMovement tankHullMovement;
    [SerializeField] TankTurretMovement tankTurretMovement;
    [SerializeField] TankWheelMovement tankWheelMovement;
    [SerializeField] TankView tankView;
    [SerializeField] TankAttack tankAttack;

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
    }

    public void TakeDamage(int damage)
    {
        if(!isDestoryed) 
        {
            currentHP -= damage;           
            if(currentHP <= 0)
            {
                currentHP = 0;
                isDestoryed = true;
                TankDestroyed();
            }
            print($"{transform.name} take Damage : [{damage}], Current HP is : {currentHP}");
        }
    }

    private void TankDestroyed()
    {        
        SetVehicleCamo(destroyedMaterial);
    }
}
