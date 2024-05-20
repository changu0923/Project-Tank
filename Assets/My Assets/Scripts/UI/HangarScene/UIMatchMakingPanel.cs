using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMatchMakingPanel : MonoBehaviour
{
    [SerializeField] Text elapsedTimeText;
    [SerializeField] Text statusText;
    [SerializeField] Text matchedPlayerText;
    [SerializeField] Button backButton;

    private float elapsedTime;
    private bool isMatchMaking;

    public Button BackButton { get => backButton; }

    private void Awake()
    {
        backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
        isMatchMaking = true;     
        StartCoroutine(StartMatchMakingCoroutine());
    }

    private void Update()
    {
        if (isMatchMaking)
        {
            elapsedTime += Time.deltaTime;
            UpdateElapsedTimeText();
        }
    }

    public void OnBackButtonClick()
    {
        PhotonNetwork.LeaveRoom();
        isMatchMaking = false;
        gameObject.SetActive(false);
    }

    private IEnumerator StartMatchMakingCoroutine()
    {
        yield return null;
    }

    private void UpdateElapsedTimeText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        elapsedTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetPlayerCount(int playerCount)
    {        
        matchedPlayerText.text = $"Matched Players : {playerCount}";
    }
}
