using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    #region Instance
    private static TestGameManager instance;

    public static TestGameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TestGameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<TestGameManager>();
                }
            }
            return instance;
        }
    }
    private void Awake()
    {       
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    public TankHullMovement tankMovement;
    public TankTurretMovement turretMovement;
    public Transform targetTransform;
}
