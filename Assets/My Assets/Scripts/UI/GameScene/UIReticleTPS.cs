using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIReticleTPS : MonoBehaviour
{ 
    [SerializeField]RectTransform inCircleReticle;
    [SerializeField]Image outCircleBackgroundReticle;
    [SerializeField]Image outCircleReticle;
    private Transform aimTransform;
    private bool isReady = false;
    public bool IsReady { get => isReady; set => isReady = value; }

    void Update()
    {
        if (isReady == true)
        {
            Vector3 screenGunAimPosition = Camera.main.WorldToScreenPoint(aimTransform.position);
            inCircleReticle.position = screenGunAimPosition;
        }
    }

    public void SetAimTransform(Transform target)
    {
        aimTransform = target;
    }

    public void StartReload(float _t)
    {
        StartCoroutine(StartReloadBar(_t));
    }
    IEnumerator StartReloadBar(float _time)
    {
        float totalTime = _time;
        float currentTime = 0f;

        while (currentTime <= totalTime)
        {
            float normalizedTime = currentTime / totalTime;
            outCircleReticle.fillAmount = normalizedTime;
            currentTime += Time.deltaTime;
            yield return null;
        }
        outCircleReticle.fillAmount = 1.0f;
    }
}
