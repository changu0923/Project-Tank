using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

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
        print($"ChangeNickname Success : [{DateTime.Now}][Name : {PhotonNetwork.NickName}]");
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
            // TODO : CountDown Start;
            // 카운트다운 시작하고, 플레이어 닉네임들 동기화 시키기
        }
    }


    #region Override Photon PunClasses
    public override void OnConnectedToMaster()
    {
        print($"Photon Login Success : [{DateTime.Now}][User : {PhotonNetwork.NickName}]");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        print($"JoinLobby Success : [{DateTime.Now}]");
    }

    // 로컬 플레이어가 방에 잘 접속했을때 호출
    public override void OnJoinedRoom()
    {        
        Debug.Log($"Player Count : {PhotonNetwork.CurrentRoom.PlayerCount} [{DateTime.Now}]");
        currentRoomPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
    }

    // 다른 플레이어가 방에 잘 접속했을때 호출
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player Joined : {PhotonNetwork.CurrentRoom.PlayerCount} [{DateTime.Now}]");
        currentRoomPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if(PhotonNetwork.IsMasterClient)
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
    }

    // https://doc-api.photonengine.com/en/pun/current/class_photon_1_1_pun_1_1_mono_behaviour_pun_callbacks.html#afb96ff9ce687e592d74866b8775f1b32
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        print($"{targetPlayer.NickName} Updated Custom Properties ({changedProps}) [{DateTime.Now}]");
        if(PhotonNetwork.IsMasterClient) 
        {
            if (changedProps.ContainsKey("SceneLoaded"))
            {
                bool sceneLoaded = (bool)changedProps["SceneLoaded"];

                if (sceneLoaded)
                {
                    loadedPlayers++;
                    CountLoadedPlayer();
                }
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
    #endregion
}