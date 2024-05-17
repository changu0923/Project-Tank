using Photon.Pun.Demo.Cockpit;
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

    private TankStat tankStat;

    public Text PlayerName { get => playerName;  }
    public Text TankName { get => tankName;  }
    public Text HpbarText { get => hpbarText;  }
    public Text CurrentSpeedText { get => currentSpeedText; }
    public Image HpbarImage { get => hpbarImage; }
    public TankStat TankStat { get => tankStat; set => tankStat = value; }

    public void InitData(TankStat tankStat, string _playerName)
    {
        if(TankStat == null) 
        {
            Debug.LogError("UIPlayerStatusPanel.cs : Tankstat is Null");
            return;
        }
        TankName.text = TankStat.TankName;
        playerName.text = _playerName;
        UpdateHPBar();
        SubscribeTakeDamage(tankStat);
    }

    public void UpdateHPBar()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(tankStat.CurrentHP);
        sb.Append(" / ");
        sb.Append(tankStat.MaxHP);
        hpbarText.text = sb.ToString();
        hpbarImage.fillAmount = (float)tankStat.CurrentHP / (float)tankStat.MaxHP; 
    }

    public void SubscribeTakeDamage(TankStat tankStat)
    {
        if (tankStat != null)
            tankStat.OnTakeDamage += UpdateHPBar;
    }
}