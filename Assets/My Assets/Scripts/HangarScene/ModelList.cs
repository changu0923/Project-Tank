using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ModelList : MonoBehaviour
{
    [SerializeField] List<GameObject> vehicleModelList = new List<GameObject>();
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    [SerializeField] List<Material> camoMaterials = new List<Material>();
    [SerializeField] List<Sprite> camoSprites = new List<Sprite>();
    [SerializeField] List<Sprite> vehicleIcons = new List<Sprite>();    

    public List<GameObject> VehicleModelList { get => vehicleModelList; set => vehicleModelList = value; }
    public List<Sprite> Sprites { get => sprites; set => sprites = value; }
    public List<Material> CamoMaterials { get => camoMaterials; set => camoMaterials = value; }
    public List<Sprite> CamoSprites { get => camoSprites; set => camoSprites = value; }

    public Sprite GetSprite(string nation)
    {
        if(nation == "USA")
        {
            return sprites[0];
        }
        else if(nation == "RUSSIA")
        {
            return sprites[1];
        }
        else
        {
            return null;
        }
    }
    
    public Sprite GetIcon(string tankName)
    {
        if (tankName == "M1")
        {
            return vehicleIcons[0];
        }
        else if (tankName == "T-72")
        {
            return vehicleIcons[1];
        }
        else
        {
            return null;
        }
    }
}
