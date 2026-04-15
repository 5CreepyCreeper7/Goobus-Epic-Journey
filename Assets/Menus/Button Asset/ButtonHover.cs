using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverScale = 1.1f;
    public float speed = 10f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        originalScale = transform.localScale;
    }

    void OnEnable() {
        transform.localScale = originalScale;
        targetScale = originalScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }

    /*void OnDisable() {
        transform.localScale = originalScale;
        targetScale = originalScale;
    }*/
}
