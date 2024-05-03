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


    // ������ Ŭ���̾�Ʈ�� ���� ����, ����� ȣ���ϱ�
    private void ChangeScene(string sceneName)
    {
       PhotonNetwork.LoadLevel(sceneName);
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

    // ���� �÷��̾ �濡 �� ���������� ȣ��
    public override void OnJoinedRoom()
    {        
        Debug.Log($"Player Count : {PhotonNetwork.CurrentRoom.PlayerCount} [{DateTime.Now}]");
        currentRoomPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
    }

    // �ٸ� �÷��̾ �濡 �� ���������� ȣ��
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