using UnityEngine;
using UnityEngine.UI;

public class BGScroll : MonoBehaviour
{
    [SerializeField] private RawImage bgImage;
    [SerializeField] private float x;
    [SerializeField] private float y;

    public TitleGoobusAnimations goobusAnimations;

    void Update()
    {
        bgImage.uvRect = new Rect(
            bgImage.uvRect.x + x * Time.deltaTime,
            bgImage.uvRect.y + y * Time.deltaTime,
            bgImage.uvRect.width,
            bgImage.uvRect.height
        );

        if(goobusAnimations.getMenuInt() == 1) {
            x = 0.1f;
        } else if(goobusAnimations.getMenuInt() == 0) {
            x = 0.05f;
        }
    }

    
}