using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region singleton
    private static EquipmentManager instance;
    public static EquipmentManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EquipmentManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("EquipmentManager");
                    instance = obj.AddComponent<EquipmentManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    [SerializeField] List<Equipment> equipmentList = new List<Equipment>(); 
    private Dictionary<int, Equipment> equipmentDic = new Dictionary<int, Equipment>();
    private Equipment[] currentEquipmentSlot = new Equipment[3];

    public Dictionary<int, Equipment> EquipmentDic { get => equipmentDic;}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        int index = 0;
        foreach (var equipment in equipmentList)
        {
            equipmentDic.Add(index, equipment);
            index++;
        }
    }

    public void SetEquipmentOnSlot(int slotIndex, Equipment equipment)
    {
        currentEquipmentSlot[slotIndex] = equipment;
    }

    public bool CheckEquipmentOnSlot(int slotIndex, Equipment equipment)
    {
        for(int i = 0; i < currentEquipmentSlot.Length; i++)
        {
            if(i != slotIndex)
            {
                if (currentEquipmentSlot[i] == equipment)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
