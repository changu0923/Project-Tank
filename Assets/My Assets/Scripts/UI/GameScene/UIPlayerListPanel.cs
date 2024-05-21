using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerListPanel : MonoBehaviour
{
    [SerializeField] RectTransform playerListHolder;
    [SerializeField] GameObject playerListElement;
    [SerializeField] Text timeText;

    private Coroutine roundTimerCoroutine;

    private Dictionary<GameObject, string> playerDic = new Dictionary<GameObject, string>();
    public void AddPlayer(string playerName)
    {
        GameObject newPlayerElement = Instantiate(playerListElement, playerListHolder);
        playerDic.Add(newPlayerElement, playerName);
        UIPlayerListComponent newPlayer = newPlayerElement.GetComponent<UIPlayerListComponent>();
        newPlayer.NicknameText.text = playerName;
        newPlayer.VehicleNameText.text = "M1";  
    }

    public void PlayerGetPoint(string playerName)
    {
        GameObject target = null;

        foreach (var targetGameobject in playerDic)
        {
            if (targetGameobject.Value == playerName)
            {
                target = targetGameobject.Key;
            }
        }

        if (target != null)
        {
            target.GetComponent<UIPlayerListComponent>().AddKillPoint();
        }
    }

    public void PlayerDestroyed(string playerName)
    {
        GameObject targetKilled = null;

        foreach (var targetGameobject in playerDic)
        {
            if (targetGameobject.Value == playerName)
            {
                targetKilled = targetGameobject.Key;
            }
        }

        if (targetKilled != null)
        {
            targetKilled.GetComponent<UIPlayerListComponent>().PlayerDestroyed();
        }
    }

    public void RemovePlayer(string playerName)
    {
        GameObject targetToDelete = null;

        foreach (var targetGameobject in playerDic)
        {
            if(targetGameobject.Value == playerName) 
            {
                targetToDelete = targetGameobject.Key;
            }
        }
        
        if(targetToDelete != null) 
        {
            Destroy(targetToDelete);
        }
    }

    public void StartRoundTimer(int _t)
    {
        roundTimerCoroutine = StartCoroutine(RoundTimerCoroutine(_t));
    }

    IEnumerator RoundTimerCoroutine(int _time)
    {
        int countdownValue = _time;
 
        while (countdownValue > 0)
        {
            if (countdownValue <= 15)
            {
                timeText.color = Color.red;
            }

            int minutes = countdownValue / 60;
            int seconds = countdownValue % 60;
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return new WaitForSeconds(1f);
            countdownValue--;
        }
        timeText.text = string.Format("{0:00}:{1:00}", 0, 0);
        if(PhotonNetwork.IsMasterClient)
        {
            GameManager.Instance.RequestTimeOver();
        }
    }
}
