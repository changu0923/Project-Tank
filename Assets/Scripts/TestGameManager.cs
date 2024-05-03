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

    [SerializeField] TextMeshProUGUI debugText;
    public TankHullMovement tankMovement;
    public TankTurretMovement turretMovement;
    public Transform targetTransform;
    public Transform gunTransform;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (debugText != null) 
        {
            StringBuilder sb = new StringBuilder();
            if(tankMovement.Z > 0f)
            {
                sb.AppendLine("Direction : Foward");
            }
            else if(tankMovement.Z < 0f)
            {
                sb.AppendLine("Direction : Backward");
            }
            else
            {
                sb.AppendLine("Direction : Stop");
            }

            if(tankMovement.X > 0f)
            {
                sb.AppendLine("Rotation : Right");
            }
            else if(tankMovement.X < 0f)
            {
                sb.AppendLine("Rotation : Left");
            }
            else
            {
                sb.AppendLine("Rotation : None");
            }
           


            // gun
            sb.AppendLine($"Gun Elevation : {(int)gunTransform.eulerAngles.x}");

            sb.AppendLine($"Atan : {(int)turretMovement.CurrentAngle}");

            debugText.text = sb.ToString();
        }
    }
}
