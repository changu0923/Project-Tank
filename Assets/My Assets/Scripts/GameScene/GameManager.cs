using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
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

    // MasterClient Only
    private Dictionary<string, bool> currentRoomPlayerDic = new Dictionary<string, bool>();

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

    #region CustomProperties
    private void SceneLoadComplete()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "SceneLoaded", true } });
    }

    private void InitPlayerStatus()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "AddPlayer", false } });
    }

    public void UpdateCurrentHealth(int currentHP)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "CurrentHP", currentHP} });
    }
    public void PlayerDestroyed(string killer)
    {
        // Killer
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "EarnPoint", killer } });

        // Victim
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "Destroyed", true } });
    }

    private void GameOverRoomPropertyUpdate()
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "GameOver", true } });
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
        InitPlayerStatus();

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
        vCam.m_LookAt = tankView.CameraRoot;    
    }

    #region MasterClient

    public void AddCurrentPlayer(string name, bool status)
    {
        currentRoomPlayerDic.Add(name, status);
    }

    public void ChangePlayerStatus(string name, bool status)
    {
        currentRoomPlayerDic[name] = status;
        CheckPlayersAlive();
    }

    private void CheckPlayersAlive()
    {
        int currentPlayerCount = currentRoomPlayerDic.Count;
        int alivePlayer = 0;
        foreach (var player in currentRoomPlayerDic)
        {
            if(player.Value == false)
            {
                alivePlayer++;
            }
        }
        if (alivePlayer <= 1)
        {
            GameOverRoomPropertyUpdate();
        }
    }

    public void GameOver()
    {
        UIManager.Instance.playerCanvas.gameOverPanel.gameObject.SetActive(true);
        bool result = tankStat.IsDestoryed;
        string panelText = result ? "You Lose." : "You Win.";
        UIManager.Instance.playerCanvas.gameOverPanel.GameOverText.text = panelText;
        PhotonManager.Instance.LeaveGameScene();
    }
    #endregion
}