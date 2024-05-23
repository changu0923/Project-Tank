using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class UICreateAccountPanel : MonoBehaviour
{
    [SerializeField] InputField emailInput;
    [SerializeField] InputField passwordInput;
    [SerializeField] InputField passwordCheckInput;
    [SerializeField] Text errorText;
    [SerializeField] Button createButton;
    [SerializeField] Button backButton;

    private void Awake()
    {
        createButton.onClick.AddListener(OnCreateButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnCreateButtonClick()
    {
        AudioManager.Instance.UIButtonClick();
        // 1. ID 중복검사
        if (DatabaseManager.Instance.CheckID(emailInput.text) == true)
        {
            errorText.text = "이미 사용중인 이메일 입니다.";
            return;
        }

        // 2. ID 공백 검사
        if (string.IsNullOrEmpty(emailInput.text) == true)
        {
            errorText.text = "이메일 입력란이 공백입니다.";
            return;
        }

        // 3. PW 공백 검사
        if (string.IsNullOrEmpty(passwordInput.text) == true || string.IsNullOrEmpty(passwordCheckInput.text))
        {
            errorText.text = "비밀번호 입력란이 공백입니다.";
            return;
        }

        // 4. PW 일치 확인
        if (string.Compare(passwordInput.text, passwordCheckInput.text) != 0)
        {
            errorText.text = "비밀번호가 불일치합니다.";
            return;
        }

        UserData userData = new UserData(emailInput.text, passwordInput.text);
        bool result = DatabaseManager.Instance.CreateUser(userData);
        if (result == true)
        {
            Debug.Log($"계정 생성 완료 : {userData.UserEmail} : [{DateTime.Now}]");
        }
        else
        {
            Debug.Log($"계정 생성 실패 : {userData.UserEmail} : [{DateTime.Now}]");
        }

        UIManager.Instance.titlePanel.loginPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnBackButtonClick()
    {
        AudioManager.Instance.UIButtonClick();
        UIManager.Instance.titlePanel.loginPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
