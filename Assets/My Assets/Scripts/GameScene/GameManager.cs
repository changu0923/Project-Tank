using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPoints;

    private List<PhotonView> activePlayers = new List<PhotonView>();

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

    private void SceneLoadComplete()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "SceneLoaded", true } });
    }
    

    private void InitializeGame()
    {
        int spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber;
        // TODO : Get Selected Vehicle From Database;
        GameObject spawnPlayer = PhotonNetwork.Instantiate("Vehicles/M1", spawnPoints[spawnIndex-1].position, spawnPoints[spawnIndex-1].rotation);

        TankStat tankStat = spawnPlayer.transform.GetComponent<TankStat>();
        SetCamera(tankStat);
        tankStat.InitializeWhenGameStart();

        Invoke("SetNickName", 2f);
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
