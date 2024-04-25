using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] List<Transform> spawnPoints;

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
        InitializeGame();
    }
    

    private void InitializeGame()
    {
        int spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber;
        GameObject spawnPlayer = PhotonNetwork.Instantiate("TestPlayer", spawnPoints[spawnIndex-1].position, spawnPoints[spawnIndex-1].rotation);
        spawnPlayer.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = PhotonNetwork.LocalPlayer.NickName;
    }
}
