using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITopBarPanel : MonoBehaviour
{
    [SerializeField] Text playerNameText;
    [SerializeField] Text playerLevelText;
    [SerializeField] Text silverText;
    [SerializeField] Button startButton;
    [SerializeField] Button editNickname;
    [SerializeField] InputField editNicknameInput;
    public Button StartButton { get => startButton;}

    private void Awake()
    {
        if (DatabaseManager.Instance.CurrentUserdata != null)
        {
            playerNameText.text = DatabaseManager.Instance.CurrentUserdata.UserNickname;
            playerLevelText.text = "LEVEL : " + DatabaseManager.Instance.CurrentUserdata.UserLevel.ToString();
            silverText.text = "Silver : " + DatabaseManager.Instance.CurrentUserdata.Silver.ToString();
        }

        startButton.onClick.AddListener(OnStartButtonClick);
        editNickname.onClick.AddListener(OnButtonEditNicknameClick);
    }

    private void OnEnable()
    {
        startButton.interactable = false;
    }

    private void OnButtonEditNicknameClick()
    {
        AudioManager.Instance.UIButtonClick();
        editNicknameInput.gameObject.SetActive(true);
        editNicknameInput.onEndEdit.RemoveAllListeners();
        editNicknameInput.onEndEdit.AddListener(OnEditNicknameInputSubmit);
    }
    
    private void OnEditNicknameInputSubmit(string name)
    {
        AudioManager.Instance.UIButtonClick();
        if (string.IsNullOrEmpty(name))
        {
            editNicknameInput.gameObject.SetActive(false);
            return;
        }

        if(DatabaseManager.Instance.CurrentUserdata != null) 
        {
            bool result = DatabaseManager.Instance.ChangeNickname(name);
            if(result) 
            {
                playerNameText.text = name;
                PhotonManager.Instance.ChangeNickname(name);
            }
        }
        editNicknameInput.gameObject.SetActive(false);
    }

    public void OnStartButtonClick()
    {
        AudioManager.Instance.UIButtonClick();
        // TODO : Start Matchmaking
        PhotonManager.Instance.JoinRandomRoom();
        startButton.interactable = false;
        UIManager.Instance.hangarPanel.matchmakingPanel.gameObject.SetActive(true);
    }

}
