using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shell_120mm : Shell
{
    public override void Fire()
    {        
        Debug.Log("Current shell damage: " + shellDamage);
    }
}
