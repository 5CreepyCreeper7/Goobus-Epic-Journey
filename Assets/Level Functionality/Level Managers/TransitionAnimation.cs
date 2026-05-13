using UnityEngine;
using System.Collections;

public class TransitionAnimation : MonoBehaviour
{
    [SerializeField] private CanvasGroup FadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1.0f;

    public IEnumerator FadeIn() {
        return Fade(1.0f, 0.0f);
    }

    public IEnumerator FadeOut() {
        return Fade(0.0f, 1.0f);
    }

    public IEnumerator Fade(float startAlpha, float endAlpha) {
        float timer = 0;

        while(timer < fadeDuration) {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            FadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            yield return null;
        }

        FadeCanvasGroup.alpha = endAlpha;
    }
}
