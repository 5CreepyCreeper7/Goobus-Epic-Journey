using UnityEngine;
using System.Collections;

public class TransitionAnimation : MonoBehaviour
{
    public static TransitionAnimation Instance { get; private set; }

    [SerializeField] private CanvasGroup FadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1.0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public IEnumerator FadeIn() {
        return Fade(1.0f, 0.0f);
    }

    public IEnumerator FadeOut() {
        return Fade(0.0f, 1.0f);
    }

    public IEnumerator Fade(float startAlpha, float endAlpha) {
        float timer = 0;

        FadeCanvasGroup.alpha = startAlpha;

        while(timer < fadeDuration) {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);

            FadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            yield return null;
        }

        FadeCanvasGroup.alpha = endAlpha;
    }
}
