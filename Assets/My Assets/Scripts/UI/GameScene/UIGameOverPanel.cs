using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverPanel : MonoBehaviour
{
    [SerializeField] Image bgImage;
    [SerializeField] Text gameOverText;
    [SerializeField] private float fadeTime = 3.0f;
    public Text GameOverText { get => gameOverText; }

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color bgColor = bgImage.color;
        Color textColor = gameOverText.color;
        bgColor.a = 0f;
        textColor.a = 0f;
        bgImage.color = bgColor;
        gameOverText.color = textColor;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = elapsedTime / fadeTime;
            alpha = Mathf.Clamp(alpha, 0f, 1f); 
            bgColor.a = alpha * 0.85f; 
            textColor.a = alpha;       
            bgImage.color = bgColor;
            gameOverText.color = textColor;
            yield return null;
        }

        bgColor.a = 0.85f;
        textColor.a = 1f;
        bgImage.color = bgColor;
        gameOverText.color = textColor;
    }
}
