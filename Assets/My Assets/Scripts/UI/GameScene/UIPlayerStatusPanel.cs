using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStatusPanel : MonoBehaviour
{
    [SerializeField] Text playerName;
    [SerializeField] Text tankName;
    [SerializeField] Text hpbarText;
    [SerializeField] Image hpbarImage;
    [SerializeField] Text currentSpeedText;

    private int currentHP;
    private int maxHP;

    public Text PlayerName { get => playerName;  }
    public Text TankName { get => tankName;  }
    public Text HpbarText { get => hpbarText;  }
    public Text CurrentSpeedText { get => currentSpeedText; }
    public Image HpbarImage { get => hpbarImage; }
    public void InitData(int _currentHP, int _maxHP, string _vehicleName, string _playerName)
    {
        currentHP = _currentHP;
        maxHP = _maxHP;
        tankName.text = _vehicleName;
        playerName.text = _playerName;
        UpdateHPBar();
    }

    public void UpdateHPBar()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(currentHP);
        sb.Append(" / ");
        sb.Append(maxHP);
        hpbarText.text = sb.ToString();
        hpbarImage.fillAmount = (float)currentHP / (float)maxHP; 
    }    
}
