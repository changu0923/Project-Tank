using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Nothing,
    Damage,
    HealthPoint,
    ReloadSpeed,
    MoveSpeed,
    InstantKill,
}

[Serializable]
public class Equipment
{
    [SerializeField] ItemType type;
    [SerializeField] float value;
    [SerializeField] string equipmentDescription;

    public ItemType Type { get => type; }
    public float Value { get => value; }
    public string EquipmentDescription { get => equipmentDescription; }
}
