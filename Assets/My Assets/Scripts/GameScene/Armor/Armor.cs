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
            currentTankStat.TakeDamage(getDamage, from, location);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NotPenetrated(int getDamage, string from, Vector3 location)
    {
        if (currentTankStat != null)
        {
            currentTankStat.TakeDamage(0, from, location);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
