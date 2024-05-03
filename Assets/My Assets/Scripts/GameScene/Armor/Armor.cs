using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField] int armorThickness;
    private TankStat currentTankStat;

    public int GetArmorThickness { get { return armorThickness; } }

    private void Awake()
    {
        currentTankStat = GetComponentInParent<TankStat>(); 
    }
    public void Penetrated(int getDamage)
    {
        if (currentTankStat != null)
        {
            currentTankStat.TakeDamage(getDamage);
        }
        else
        {
            Destroy(gameObject);
        }
    }
 
}
