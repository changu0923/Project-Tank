using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerCanvas : MonoBehaviour
{
    public UIPlayerStatusPanel playerStatusPanel;
    public UIPlayerListPanel playerListPanel;
    public UIGameOverPanel gameOverPanel;
    public Text waitingText;
    public Image hitMarker;
    private void Awake()
    {
        UIManager.Instance.playerCanvas = this;
    }      

    public void CountdownStart()
    {
        StartCoroutine(CountdownCoroutine(5));  
    }

    public void ActiveHitMarker()
    {
        hitMarker.gameObject.SetActive(true);
        Color color = hitMarker.color;
        color.a = 1f;
        hitMarker.color = color;
        StartCoroutine(HitMarkerStart());
        AudioManager.Instance.PlayerHitEffect();
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

    IEnumerator HitMarkerStart()
    {
        float lifeTime = 0.5f;
        float elapsedTime = 0f;
        while (elapsedTime < lifeTime)
        {
            elapsedTime += Time.deltaTime;
            Color color = hitMarker.color;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / lifeTime);
            hitMarker.color = color;
            yield return null;
        }
        hitMarker.gameObject.SetActive(false);
    }
}
