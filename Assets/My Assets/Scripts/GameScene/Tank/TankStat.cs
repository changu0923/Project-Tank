using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TankStat : MonoBehaviour
{
    [SerializeField] string tankName;
    [SerializeField] int maxHP;
    [SerializeField] int maxSpeed;
    private int currentHP;
    private int currentSpeed;
    private bool isDestoryed;

    [Header("TankComponents")]
    [SerializeField] TankHullMovement tankHullMovement;
    [SerializeField] TankTurretMovement tankTurretMovement;
    [SerializeField] TankWheelMovement tankWheelMovement;
    [SerializeField] TankView tankView;
    [SerializeField] TankAttack tankAttack;

    [Header("Camo")]
    [SerializeField] List<MeshRenderer> targetRenderers = new List<MeshRenderer>();
    private Material currentCamo;
    private Material destroyedMaterial;

    [Header("Hit Information")]
    [SerializeField] GameObject uiDamageIndicator;
    private PhotonView photonView;
    public Canvas playerUICanvas;

    private void Awake()
    {    
        destroyedMaterial = Resources.Load<Material>("MaterialDestroyed"); 
        currentHP = maxHP;
        isDestoryed = false;        
    }

    public void SetVehicleCamo(Material inputMaterial)
    {
        currentCamo = inputMaterial;
        foreach (var renderer in targetRenderers) 
        { 
            renderer.material = currentCamo;  
        }
    }

    public void RequestTakeDamage(int damage, string from, Vector3 location)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            TakeDamage(damage, from, location);
        }
        else
        {
            photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage, from, location);
        }
    }

    public void TakeDamage(int damage, string from, Vector3 location)
    {
        if(!isDestoryed) 
        {
            currentHP -= damage;
            if (photonView != null)
            {
                if (photonView.IsMine == true)
                {
                    GameObject indicator = Instantiate(uiDamageIndicator, playerUICanvas.transform);
                    indicator.GetComponent<UIDamageIndicator>().SetIndicatorInfo(from, damage, location, transform);
                }
            }
            if(currentHP <= 0)
            {
                currentHP = 0;
                isDestoryed = true;
                TankDestroyed();
            }
            print($"{transform.name} take Damage : [{damage}], Current HP is : {currentHP}");
        }
    }

    private void TankDestroyed()
    {        
        SetVehicleCamo(destroyedMaterial);
    }

    public void InitializeWhenGameStart()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine == true)
        {
            playerUICanvas = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<Canvas>();
            bool result = playerUICanvas != null;
            tankHullMovement.enabled = true;
            tankTurretMovement.enabled = true;
            tankAttack.ScriptOn = true;
        }
    }
}
