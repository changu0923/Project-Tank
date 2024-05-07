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
        print($"SceneLoadComplete {PhotonNetwork.LocalPlayer.NickName} Updated Custom Properties [{DateTime.Now}]");
    }
    

    private void InitializeGame()
    {
        int spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber;
        GameObject spawnPlayer = PhotonNetwork.Instantiate("Vehicles/M1", spawnPoints[spawnIndex-1].position, spawnPoints[spawnIndex-1].rotation);
        Invoke("SetNickName", 2f);
    }

    private void SetNickName()
    {
        PhotonView[] players = FindObjectsOfType<PhotonView>();

        foreach (PhotonView view in players)
        {            
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

    private void SetCamera()
    {
        // TODO : PhotonView == isMine 인 플레이어 찾아서 CM VCAM 할당시키기
    }
}
