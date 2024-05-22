using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    #region Singleton
    private static PhotonManager instance;
    public static PhotonManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PhotonManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("PhotonManager");
                    instance = obj.AddComponent<PhotonManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    private int currentRoomPlayerCount = 0; 
    private int loadedPlayers = 0;
    private Coroutine startGameCoroutine;

    private bool isLeaveRoom;

    private Dictionary<string, int> playerCamoIndex = new Dictionary<string, int>();
    public Dictionary<string, int> PlayerCamoIndex { get => playerCamoIndex;}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void PhotonLogin()
    {
        string uid;
        if (DatabaseManager.Instance.CurrentUserdata != null)
        {
            uid = DatabaseManager.Instance.CurrentUserdata.Uid;
            PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues(uid);
            PhotonNetwork.NickName = DatabaseManager.Instance.CurrentUserdata.UserNickname;
        }
        else
        {
            uid = $"Guest {UnityEngine.Random.Range(100, 1000)}";
            PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues(uid);
            PhotonNetwork.NickName = uid;
        }
        print($"Trying Photon Login : [{DateTime.Now}]");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void PhotonLogout()
    {
        PhotonNetwork.Disconnect();
    }

    public void JoinRandomRoom()
    {
        RoomOptions option = new RoomOptions();
        option.MaxPlayers = 8;
        string createRoomName = $"{PhotonNetwork.NickName}'s Room";
        PhotonNetwork.JoinRandomOrCreateRoom(roomName: createRoomName, roomOptions: option);
    }
    public void ChangeNickname(string newNickname)
    {
        PhotonNetwork.NickName = newNickname;
    }


    // 마스터 클라이언트가 게임 시작, 종료시 호출하기
    private void ChangeScene(string sceneName)
    {
       PhotonNetwork.LoadLevel(sceneName);
    }

    // 게임신 로딩이 끝난 플레이어 수 세기
    private void CountLoadedPlayer()
    {
        if( currentRoomPlayerCount == loadedPlayers) 
        {            
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "AllPlayerReady", true } });
        }
    }

    private void SetCurrentVehicleCamo()
    {
        if (DatabaseManager.Instance.SelectedTank != null)
        {
            int camoIndex = DatabaseManager.Instance.SelectedTank.CamoSlot;
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "Camo", camoIndex } });
            print("SetCurrentVehicleCamo() : OK");
        }
        else
        {
            int camoIndex = 0;
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "Camo", camoIndex } });
            print("SetCurrentVehicleCamo() : Failed");
        }
    }

    private void UpdatePlayersVehicleCamo()
    {
        foreach (Player player in PhotonNetwork.PlayerList) 
        {
            if(player.CustomProperties.ContainsKey("Camo"))
            {
                int index = (int)player.CustomProperties["Camo"];
                playerCamoIndex[player.NickName] = index;
                print($"UpdatePlayersVehicleCamo() : {player.NickName} found Camo : [{index}] ");
            }
        }      
    }

    // 게임신에서 플레이어가 모두 접속완료하였으면, 플레이어 리스트 생성
    private void InitPlayerListGameScene()
    {
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            {
                Player photonPlayer = player.Value;
                UIManager.Instance.playerCanvas.playerListPanel.AddPlayer(photonPlayer.NickName);
            }
        }
    }

    public void LeaveGameScene()
    {
        StartCoroutine(LeaveRoom());
    }

    #region Override Photon Pun Classes
    public override void OnConnectedToMaster()
    {
        print($"Photon Login Success : [{DateTime.Now}][User : {PhotonNetwork.NickName}]");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        print($"JoinLobby Success : [{DateTime.Now}]");
        UIManager.Instance.hangarPanel.topBarPanel.StartButton.interactable = true;
    }

    // 로컬 플레이어가 방에 잘 접속했을때 호출
    public override void OnJoinedRoom()
    {
        SetCurrentVehicleCamo();
        currentRoomPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        UIManager.Instance.hangarPanel.matchmakingPanel.SetPlayerCount(currentRoomPlayerCount);
        Invoke("UpdatePlayersVehicleCamo", 2f);
    }

    // 다른 플레이어가 방에 잘 접속했을때 호출
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {        
        currentRoomPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        UIManager.Instance.hangarPanel.matchmakingPanel.SetPlayerCount(currentRoomPlayerCount);
        if (PhotonNetwork.IsMasterClient)
        {
            if(currentRoomPlayerCount > 1)
            {
                if(startGameCoroutine != null)
                {
                    StopCoroutine(startGameCoroutine);
                }
                startGameCoroutine = StartCoroutine(StartGameCoroutine("GameScene", 5f));
            }
        }
    }

    public override void OnLeftRoom()
    {
        currentRoomPlayerCount--;

        if(isLeaveRoom == true)
        {
            SceneManager.LoadScene("HangarScene");
            isLeaveRoom = false;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(playerCamoIndex.ContainsKey(otherPlayer.NickName))
        {
            playerCamoIndex.Remove(otherPlayer.NickName);
        }
    }

    // references https://doc-api.photonengine.com/en/pun/current/class_photon_1_1_pun_1_1_mono_behaviour_pun_callbacks.html#afb96ff9ce687e592d74866b8775f1b32
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(PhotonNetwork.IsMasterClient) 
        {
            #region SceneLoaded
            if (changedProps.ContainsKey("SceneLoaded"))
            {
                bool sceneLoaded = (bool)changedProps["SceneLoaded"];

                if (sceneLoaded)
                {
                    loadedPlayers++;
                    CountLoadedPlayer();
                }
            }
            #endregion
            #region SetCamo
            if (changedProps.ContainsKey("Camo"))
            {
                int index = (int)changedProps["Camo"]; 
                if (playerCamoIndex.ContainsKey(targetPlayer.NickName))
                {
                    playerCamoIndex[targetPlayer.NickName] = index;
                }
                else
                {
                    playerCamoIndex.Add(targetPlayer.NickName, index);
                }
            }
            #endregion
            #region Add Player into Dictionary

            if (changedProps.ContainsKey("AddPlayer"))
            {
                bool result = (bool)changedProps["AddPlayer"];
                GameManager.Instance.AddCurrentPlayer(targetPlayer.NickName, result);
            }
            #endregion
            #region HP Update
            if (changedProps.ContainsKey("CurrentHP"))
            {
                int currentHP = (int)changedProps["CurrentHP"];
                if(currentHP <=0)
                {
                    currentHP = 0;
                }
            }
            #endregion

        }

        #region AttackSuccessLog
        if (changedProps.ContainsKey("AttackSuccessLog"))
        {
            string log = (string)changedProps["AttackSuccessLog"];
            string[] names = log.Split('|');
            if (names.Length >= 2)
            {
                string attackerName = names[0];
                string targetName = names[1];
                GameManager.Instance.HitMarkerCheck(attackerName);
            }
        }
        #endregion
        #region AttackFailedLog
        if (changedProps.ContainsKey("AttackFailedLog"))
        {
            string log = (string)changedProps["AttackFailedLog"];
            string[] names = log.Split('|');
            if (names.Length >= 2)
            {
                string attackerName = names[0];
                string targetName = names[1];
                GameManager.Instance.AttackFailed(attackerName);
            }
        }
        #endregion
        if (changedProps.ContainsKey("Destroyed"))
        {
            bool result = (bool)changedProps["Destroyed"];
            if(result)
            {
                UIManager.Instance.playerCanvas.playerListPanel.PlayerDestroyed(targetPlayer.NickName);
                if (PhotonNetwork.IsMasterClient)
                {
                    GameManager.Instance.ChangePlayerStatus(targetPlayer.NickName, result);
                }
            }
        }

        if (changedProps.ContainsKey("EarnPoint"))
        {
            string result = (string)changedProps["EarnPoint"];

            UIManager.Instance.playerCanvas.playerListPanel.PlayerGetPoint(result);
        }
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if(propertiesThatChanged.ContainsKey("AllPlayerReady"))
        {
            bool result = (bool)propertiesThatChanged["AllPlayerReady"];
            if(result)
            {
                GameManager.Instance.StartCountDown();
                InitPlayerListGameScene();
            }
        }

        if (propertiesThatChanged.ContainsKey("GameOver"))
        {
            bool result = (bool)propertiesThatChanged["GameOver"];
            if (result)
            {
                GameManager.Instance.GameOver();
            }
        }

        if (propertiesThatChanged.ContainsKey("TimeOver"))
        {
            bool result = (bool)propertiesThatChanged["TimeOver"];
            if (result)
            {
                GameManager.Instance.IsTimeOver = true;
                GameManager.Instance.GameOver();
            }
        }
    }
    #endregion


    #region Coroutines
    IEnumerator StartGameCoroutine(string sceneName, float waitTime) 
    {    
        yield return new WaitForSeconds(waitTime);
        startGameCoroutine = null;
        ChangeScene(sceneName);
    }

    IEnumerator LeaveRoom()
    {
        print("LeaveRoom() Called");
        yield return new WaitForSeconds(10f);
        PhotonNetwork.LeaveRoom();
        print("LeaveRoom() Complete");
        isLeaveRoom = true;
    }
    #endregion
}