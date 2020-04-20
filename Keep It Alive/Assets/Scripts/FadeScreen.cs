using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private GameObject dieText;
    [SerializeField] private GameObject endText;

    // Fade screen color

    public void StartFade(Color oldColor, Color newColor, float duration)
    {
        StartCoroutine(FadeToColor(oldColor, newColor, duration));
    }

    IEnumerator FadeToColor(Color oldColor, Color newColor, float duration)
    {
        float step = 1f / duration;
        for (float i = 0f; i < 1f; i += step * Time.deltaTime)
        {
            fadeScreen.color = Color.Lerp(oldColor, newColor, i);
            yield return null;
        }
    }

    public void ShowDieText(float duration)
    {
        StartCoroutine(ShowText(dieText.gameObject, duration));
    }

    public void ShowEndText(float duration)
    {
        StartCoroutine(ShowText(endText.gameObject, duration));
    }

    IEnumerator ShowText(GameObject text, float duration)
    {

        Image[] children = text.GetComponentsInChildren<Image>();

        foreach (Image child in children)
        {
            child.enabled = true;
        }

        yield return new WaitForSeconds(duration);

        foreach (Image child in children)
        {
            child.enabled = false;
        }
    }
}
