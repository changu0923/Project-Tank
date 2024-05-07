using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        InitializeGame();
    }
    

    private void InitializeGame()
    {
        int spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber;
        GameObject spawnPlayer = PhotonNetwork.Instantiate("Vehicles/M1", spawnPoints[spawnIndex-1].position, spawnPoints[spawnIndex-1].rotation);
        Invoke("SetNickName", 2f);
    }

    private void SetNickName()
    {
        // 모든 PhotonView를 가져옵니다.
        PhotonView[] players = FindObjectsOfType<PhotonView>();

        foreach (PhotonView view in players)
        {            
            print(view.Owner.NickName);
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
     
    }
}
