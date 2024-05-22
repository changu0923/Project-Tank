using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPoints;

    private List<PhotonView> activePlayers = new List<PhotonView>();
    private TankStat tankStat;
    private PhotonView photonView;

    private Dictionary<string, bool> currentRoomPlayerDic = new Dictionary<string, bool>();
    private bool isTimeOver;
    private string playerName;
    public bool IsTimeOver { get => isTimeOver; set => isTimeOver = value; }
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
    public void SendAttackSuccessLog(string from, string target)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(from);
        sb.Append('|');
        sb.Append(target);
        string log = sb.ToString();
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "AttackSuccessLog", log } });
    }

    public void SendAttackFailedLog(string from, string target)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(from);
        sb.Append('|');
        sb.Append(target);
        string log = sb.ToString();
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "AttackFailedLog", log } });
    }

    private void GameOverRoomPropertyUpdate()
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "GameOver", true } });
    }

    private void TimeOverRoomPropertyUpdate()
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "TimeOver", true } });
    }

    public void RequestTimeOver()
    {
        TimeOverRoomPropertyUpdate();
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
        Invoke("SetPlayerInfo", 2.5f);
    }

    public void StartCountDown()
    {
        UIManager.Instance.playerCanvas.CountdownStart();
    }

    public void InitalizeAfterCountDown()
    {
        tankStat.InitializeWhenGameStart();
        UIManager.Instance.playerCanvas.playerListPanel.StartRoundTimer(300);
    }

    private void SetPlayerInfo()
    {
        PhotonView[] players = FindObjectsOfType<PhotonView>();
        string myNickName = photonView.Owner.NickName;
        foreach (PhotonView view in players)
        {
            view.gameObject.name = view.Owner.NickName + "'s Vehicle";
            SetCamo(view);
            Canvas canvas = view.GetComponentInChildren<Canvas>();
            if (canvas != null)
            {                
                TextMeshProUGUI textMeshPro = canvas.GetComponentInChildren<TextMeshProUGUI>();
                if (textMeshPro != null)
                {
                    string playerName = view.Owner.NickName;
                    if (playerName != myNickName)
                    {
                        textMeshPro.text = playerName;
                    }
                    else
                    {
                        textMeshPro.text = " ";
                        this.playerName = playerName;
                    }
                }
            }          
        }
    }

    public void HitMarkerCheck(string attacker)
    {
        if(photonView != null)
        {
            if(attacker == playerName)
            {
                UIManager.Instance.playerCanvas.ActiveHitMarker();
            }
        }
    }
    public void AttackFailed(string attacker)
    {
        if (photonView != null)
        {
            if (attacker == playerName)
            {
                //TODO : 도탄되었습니다.
            }
        }
    }

    private void SetCamo(PhotonView player)
    {
        PhotonView target = player;
        TankStat currentPlayerStat = target.GetComponent<TankStat>();
        int index = PhotonManager.Instance.PlayerCamoIndex[target.Owner.NickName];
        Material mat = UIManager.Instance.hangarPanel.ModelList.CamoMaterials[index];
        currentPlayerStat.SetVehicleCamo(mat);
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
            if(isTimeOver == true)
            {
                return;
            }
            GameOverRoomPropertyUpdate();
        }
    }

    #endregion
    public void GameOver()
    {
        UIManager.Instance.playerCanvas.gameOverPanel.gameObject.SetActive(true);
        bool result = tankStat.IsDestoryed;
        string panelText = result ? "You Lose." : "You Win.";
        if(isTimeOver == true)
        {
            panelText = "Time Over";
        }
        UIManager.Instance.playerCanvas.gameOverPanel.GameOverText.text = panelText;
        PhotonManager.Instance.LeaveGameScene();
    }
}