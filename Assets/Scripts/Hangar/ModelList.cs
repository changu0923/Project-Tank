using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ModelList : MonoBehaviour
{
    [SerializeField] List<GameObject> vehicleModelList = new List<GameObject>();
    [SerializeField] List<Sprite> sprites = new List<Sprite>();

    public List<GameObject> VehicleModelList { get => vehicleModelList; set => vehicleModelList = value; }
    public List<Sprite> Sprites { get => sprites; set => sprites = value; }

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
}
