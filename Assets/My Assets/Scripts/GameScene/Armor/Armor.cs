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
    public void Penetrated(int getDamage, string from, Vector3 location)
    {
        if (currentTankStat != null)
        {
            print($" Penetrated() : {getDamage}, {from}, {location}");
            currentTankStat.TakeDamage(getDamage, from, location);
        }
        else
        {
            Destroy(gameObject);
        }
    }
 
}
