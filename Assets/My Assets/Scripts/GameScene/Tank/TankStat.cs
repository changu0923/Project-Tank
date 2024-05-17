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

    public int CurrentHP { get => currentHP; }
    public int MaxHP { get => maxHP; }
    public string TankName { get => tankName; }


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
       
    // TODO : 내가 포탄을 맞혀서 나온 결과를 각 클라이언트가 커스텀프로퍼티 업데이트. 
    public void TakeDamage(int damage, string from, Vector3 location)
    {
        if(photonView == null) 
        {
            return;
        }        

        if(!isDestoryed) 
        {
            currentHP -= damage;
            if (photonView != null)
            {
                if (photonView.IsMine == true)
                {
                    GameObject indicator = Instantiate(uiDamageIndicator, playerUICanvas.transform);
                    indicator.GetComponent<UIDamageIndicator>().SetIndicatorInfo(from, damage, location, tankView.CameraRoot);
                    GameManager.Instance.UpdateCurrentHealth(this.currentHP);
                }
            }
            if(currentHP <= 0)
            {
                currentHP = 0;
                isDestoryed = true;
                photonView.RPC("TankDestroyed", RpcTarget.All);
            }
        }
    }

    [PunRPC]
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
            tankHullMovement.enabled = true;
            tankTurretMovement.enabled = true;
            tankAttack.ScriptOn = true;
        }
    }
}
