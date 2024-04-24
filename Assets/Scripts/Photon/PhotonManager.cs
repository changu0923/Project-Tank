using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
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

    public override void OnConnectedToMaster()
    {
        print($"Photon Login Success : [{DateTime.Now}][User : {PhotonNetwork.NickName}]");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        print($"JoinLobby Success : [{DateTime.Now}]");
    }

    public override void OnLeftRoom()
    {
        print($"Leave Room : [{DateTime.Now}]");
    }

    public void ChangeNickname(string newNickname)
    {
        PhotonNetwork.NickName = newNickname;
        print($"ChangeNickname Success : [{DateTime.Now}][Name : {PhotonNetwork.NickName}]");
    }
}
