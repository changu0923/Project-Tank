using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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

    [Header("VFX")]
    [SerializeField] GameObject destroyedVFX;
    private AudioClip armorPenetratedClip;
    private AudioClip armorDeflectedClip;
    private AudioClip vehicleDestroyedClip;
    private AudioSource audioSource;


    private Action onTakeDamage;

    
    public int CurrentHP { get => currentHP; }
    public int MaxHP { get => maxHP; }
    public string TankName { get => tankName; }
    public Action OnTakeDamage { get => onTakeDamage; set => onTakeDamage = value; }
    public bool IsDestoryed { get => isDestoryed; }

    private void Awake()
    {    
        destroyedMaterial = Resources.Load<Material>("MaterialDestroyed"); 
        audioSource = GetComponent<AudioSource>();
        currentHP = maxHP;
        isDestoryed = false;
        armorPenetratedClip = AudioManager.Instance.penetratedClip;
        armorDeflectedClip = AudioManager.Instance.deflectedClip;
        vehicleDestroyedClip = AudioManager.Instance.explosionClip;

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
                    if (damage != 0)
                    {
                        GameManager.Instance.SendAttackSuccessLog(from, photonView.Owner.NickName);
                        photonView.RPC("ArmorPenetrated", RpcTarget.All);
                    }
                    else
                    {
                        GameManager.Instance.SendAttackFailedLog(from, photonView.Owner.NickName);
                        photonView.RPC("ArmorPenetrateFailed", RpcTarget.All);
                    }
                }
            }
            if(currentHP <= 0)
            {
                currentHP = 0;
                isDestoryed = true;
                photonView.RPC("TankDestroyed", RpcTarget.All);
                GameManager.Instance.PlayerDestroyed(from);
            }
            OnTakeDamage?.Invoke();
        }
    }

    [PunRPC]
    private void ArmorPenetrated()
    {
        AudioManager.Instance.PlayAudio(armorPenetratedClip, transform);
    }

    [PunRPC]
    private void ArmorPenetrateFailed()
    {
        AudioManager.Instance.PlayAudio(armorDeflectedClip, transform);
    }

    [PunRPC]
    private void TankDestroyed()
    {
        GameObject vfx = Instantiate(destroyedVFX);
        AudioManager.Instance.PlayAudio(vehicleDestroyedClip, transform);
        vfx.transform.position = transform.position;
        vfx.transform.rotation = Quaternion.identity;
        Destroy(vfx, 10f);
        SetVehicleCamo(destroyedMaterial);

        if (photonView != null)
        {
            if (photonView.IsMine == true)
            {
                tankHullMovement.enabled = false;
                tankTurretMovement.enabled = false; 
                tankAttack.ScriptOn = false;
                tankView.ActiveDeathCam();
            }
        }
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
