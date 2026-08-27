using System.Collections;
using UnityEngine;
using System;

public class RecordUIAnimation : MonoBehaviour
{
    public event Action OnCloseComplete;

    [Header("Menu Animation")]
    public RectTransform topGroup;     // Text (TMP) title
    public RectTransform shelfGroup;   // Scroll View
    public RectTransform bottomGroup;  // RecordPlayer, slider, buttons

    public CanvasGroup menuCanvasGroup;

    public float slideDuration = 0.4f;
    public AnimationCurve slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector2 shelfOnPos, topOnPos, bottomOnPos;
    private Vector2 shelfOffPos, topOffPos, bottomOffPos;
    private bool positionsInitialized = false;

    private Coroutine menuAnimCoroutine;

    private void Start() {
        InitializePositions();
    }

    private void InitializePositions() {
        if (positionsInitialized) {
            return;
        }

        // capture the designed "on-screen" positions, then compute off-screen offsets
        shelfOnPos = shelfGroup.anchoredPosition;
        topOnPos = topGroup.anchoredPosition;
        bottomOnPos = bottomGroup.anchoredPosition;

        shelfOffPos = shelfOnPos + new Vector2(shelfGroup.rect.width, 0f);    // off the right edge
        topOffPos = topOnPos + new Vector2(0f, topGroup.rect.height);          // off the top edge
        bottomOffPos = bottomOnPos - new Vector2(0f, bottomGroup.rect.height); // off the bottom edge

        positionsInitialized = true;
    }

    public void OpenMenu() {
        InitializePositions(); // safety net in case Start() ran before layout settled

        gameObject.SetActive(true);
        menuCanvasGroup.blocksRaycasts = true;

        // snap to off-screen start before animating in
        shelfGroup.anchoredPosition = shelfOffPos;
        topGroup.anchoredPosition = topOffPos;
        bottomGroup.anchoredPosition = bottomOffPos;

        if (menuAnimCoroutine != null) {
            StopCoroutine(menuAnimCoroutine);
        }
        menuAnimCoroutine = StartCoroutine(AnimateMenu(true));
    }

    public void CloseMenu() {
        menuCanvasGroup.blocksRaycasts = false;

        if (menuAnimCoroutine != null) {
            StopCoroutine(menuAnimCoroutine);
        }
        menuAnimCoroutine = StartCoroutine(AnimateMenu(false));
    }

    private IEnumerator AnimateMenu(bool opening) {
        float t = 0f;

        while (t < slideDuration) {
            t += Time.unscaledDeltaTime; // unscaled in case opening this menu pauses the game
            float normalized = slideCurve.Evaluate(t / slideDuration);
            float progress = opening ? normalized : 1f - normalized;

            shelfGroup.anchoredPosition = Vector2.LerpUnclamped(shelfOffPos, shelfOnPos, progress);
            topGroup.anchoredPosition = Vector2.LerpUnclamped(topOffPos, topOnPos, progress);
            bottomGroup.anchoredPosition = Vector2.LerpUnclamped(bottomOffPos, bottomOnPos, progress);

            yield return null;
        }

        Vector2 finalShelf = opening ? shelfOnPos : shelfOffPos;
        Vector2 finalTop = opening ? topOnPos : topOffPos;
        Vector2 finalBottom = opening ? bottomOnPos : bottomOffPos;

        shelfGroup.anchoredPosition = finalShelf;
        topGroup.anchoredPosition = finalTop;
        bottomGroup.anchoredPosition = finalBottom;

        if (!opening) {
            gameObject.SetActive(false);
            OnCloseComplete?.Invoke();
        }
    }
}