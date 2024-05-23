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
        // 1. ID �ߺ��˻�
        if (DatabaseManager.Instance.CheckID(emailInput.text) == true)
        {
            errorText.text = "�̹� ������� �̸��� �Դϴ�.";
            return;
        }

        // 2. ID ���� �˻�
        if (string.IsNullOrEmpty(emailInput.text) == true)
        {
            errorText.text = "�̸��� �Է¶��� �����Դϴ�.";
            return;
        }

        // 3. PW ���� �˻�
        if (string.IsNullOrEmpty(passwordInput.text) == true || string.IsNullOrEmpty(passwordCheckInput.text))
        {
            errorText.text = "��й�ȣ �Է¶��� �����Դϴ�.";
            return;
        }

        // 4. PW ��ġ Ȯ��
        if (string.Compare(passwordInput.text, passwordCheckInput.text) != 0)
        {
            errorText.text = "��й�ȣ�� ����ġ�մϴ�.";
            return;
        }

        UserData userData = new UserData(emailInput.text, passwordInput.text);
        bool result = DatabaseManager.Instance.CreateUser(userData);
        if (result == true)
        {
            Debug.Log($"���� ���� �Ϸ� : {userData.UserEmail} : [{DateTime.Now}]");
        }
        else
        {
            Debug.Log($"���� ���� ���� : {userData.UserEmail} : [{DateTime.Now}]");
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
