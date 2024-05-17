using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerCanvas : MonoBehaviour
{
    [SerializeField] UIPlayerStatusPanel playerStatusPanel;
    [SerializeField] UIPlayerListPanel playerListPanel;
    [SerializeField] Text waitingText;
    private void Awake()
    {
        UIManager.Instance.playerCanvas = this;
    }      

    public void CountdownStart()
    {
        StartCoroutine(CountdownCoroutine(5));
    }

    IEnumerator CountdownCoroutine(int _time)
    {
        StringBuilder sb = new StringBuilder();
        int countdownValue = _time;
        while (countdownValue > 0)
        {
            sb.Append("Game will start in ");
            sb.Append(countdownValue.ToString());
            sb.Append(" seconds.");
            waitingText.text = sb.ToString();
            yield return new WaitForSeconds(1f);
            countdownValue--;
            sb.Clear();
        }
        waitingText.gameObject.SetActive(false);
        GameManager.Instance.InitalizeAfterCountDown();
    }
}
