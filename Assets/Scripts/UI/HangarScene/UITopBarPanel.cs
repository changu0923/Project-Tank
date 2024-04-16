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

    private void Awake()
    {
        playerNameText.text = DatabaseManager.Instance.CurrentUserdata.UserNickname;
        playerLevelText.text = "LEVEL : " + DatabaseManager.Instance.CurrentUserdata.UserLevel.ToString();
        silverText.text = "Silver : " + DatabaseManager.Instance.CurrentUserdata.Silver.ToString();
    }

}
