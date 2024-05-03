using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILoginPanel : MonoBehaviour
{
    [SerializeField] InputField emailInput;
    [SerializeField] InputField passwordInput;
    [SerializeField] Button startButton;
    [SerializeField] Button createButton;
    [SerializeField] Text errorText;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        createButton.onClick.AddListener(OnCreateButtonClick);
    }

    private void OnStartButtonClick()
    {
        bool result = DatabaseManager.Instance.Login(emailInput.text, UserData.HashPassword(passwordInput.text));
        if (result) 
        {
            PhotonManager.Instance.PhotonLogin();
            SceneManager.LoadScene("HangarScene");
        }
        else
        {
            errorText.text = "�α��� ����. ���̵�� ��й�ȣ�� �ٽ� Ȯ���Ͻʽÿ�.";
        }

    }

    private void OnCreateButtonClick()
    {
        UIManager.Instance.titlePanel.createAccountPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
