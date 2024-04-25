using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    Material material;
    [SerializeField] int armorThickness;

    public int GetArmorThickness { get { return armorThickness; } }

    private void Awake()
    {
        material = GetComponent<Material>();
    }
    public void Penetrated()
    {
        print(transform.name + "is Penetrated : [" + DateTime.Now + "]");
    }    
}
