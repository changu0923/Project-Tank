using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPoints;

    private List<PhotonView> activePlayers = new List<PhotonView>();
    private TankStat tankStat;
    private PhotonView photonView;
    public PhotonView PhotonView { get => photonView;}

    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }
    #endregion
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        SceneLoadComplete();
        InitializeGame();
    }

    #region Custom Propeerties
    private void SceneLoadComplete()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "SceneLoaded", true } });
    }

    public void UpdateCurrentHealth(int currentHP)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "CurrentHP", currentHP} });
    }

    #endregion
    public void InitializeGame()
    {
        int spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber;
        // TODO : Get Selected Vehicle From Database;
        GameObject spawnPlayer = PhotonNetwork.Instantiate("Vehicles/M1", spawnPoints[spawnIndex - 1].position, spawnPoints[spawnIndex - 1].rotation);
        tankStat = spawnPlayer.transform.GetComponent<TankStat>();
        photonView = spawnPlayer.transform.GetComponent<PhotonView>();
        if (photonView == null)
        {
            Debug.LogError($"{gameObject.name} : PhotonView is Null");
        }
        if (UIManager.Instance.playerCanvas == null)
        {
            UIManager.Instance.playerCanvas = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<UIPlayerCanvas>();
            UIManager.Instance.playerCanvas.playerStatusPanel.TankStat = tankStat;
        }
        UIManager.Instance.playerCanvas.playerStatusPanel.InitData(tankStat, photonView.Owner.NickName);

        //UIManager.Instance.playerCanvas.playerStatusPanel.InitData(tankStat.CurrentHP, tankStat.MaxHP, tankStat.TankName, photonView.Owner.NickName);

        SetCamera(tankStat);
        Invoke("SetNickName", 2.5f);
    }

    public void StartCountDown()
    {
        UIManager.Instance.playerCanvas.CountdownStart();
    }

    public void InitalizeAfterCountDown()
    {
        tankStat.InitializeWhenGameStart();
    }

    private void SetNickName()
    {
        PhotonView[] players = FindObjectsOfType<PhotonView>();

        foreach (PhotonView view in players)
        {
            view.gameObject.name = view.Owner.NickName + "'s Vehicle";
            Canvas canvas = view.GetComponentInChildren<Canvas>();
            if (canvas != null)
            {                
                TextMeshProUGUI textMeshPro = canvas.GetComponentInChildren<TextMeshProUGUI>();
                if (textMeshPro != null)
                {                    
                    string playerName = view.Owner.NickName;
                    textMeshPro.text = playerName;
                }
            }          
        }
    }

    private void SetCamera(TankStat tankStatComponent)
    {
        TankView tankView = tankStatComponent.transform.GetComponent<TankView>();
        CinemachineVirtualCamera vCam = GameObject.FindGameObjectWithTag("TPSCamera").GetComponent<CinemachineVirtualCamera>();
        tankView.Vcam = vCam;
        vCam.m_Follow = tankView.CameraRoot;
    }
}
