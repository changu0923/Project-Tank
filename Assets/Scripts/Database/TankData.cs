using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class TankData
{
    private int tankID; 
    private string tankName;
    private string tankNation;
    private int tankPrice;
    private string tankDescription;
    private int itemSlot_01;
    private int itemSlot_02;
    private int itemSlot_03;

    public int TankID { get => tankID; set => tankID = value; }
    public string TankName { get => tankName; set => tankName = value; }
    public string TankNation { get => tankNation; set => tankNation = value; }
    public int TankPrice { get => tankPrice; set => tankPrice = value; }
    public string TankDescription { get => tankDescription; set => tankDescription = value; }
    public int ItemSlot_01 { get => itemSlot_01; set => itemSlot_01 = value; }
    public int ItemSlot_02 { get => itemSlot_02; set => itemSlot_02 = value; }
    public int ItemSlot_03 { get => itemSlot_03; set => itemSlot_03 = value; }

    public TankData() { }
}
