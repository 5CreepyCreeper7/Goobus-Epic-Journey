using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSFX : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public enum ButtonType { CloseMenu, ButtonClick, OpenMenu }

    private MenuSoundFX menuSFX;

    public ButtonType buttonType;

    private void Awake() {
        menuSFX = FindFirstObjectByType<MenuSoundFX>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        menuSFX.PlayHoverSound();
    }

    public void OnPointerClick(PointerEventData eventData) {
        switch(buttonType) {
            case ButtonType.CloseMenu:
                menuSFX.PlayBackSound();
                break;
            case ButtonType.ButtonClick:
                menuSFX.PlayButtonClick();
                break;
            case ButtonType.OpenMenu:
                menuSFX.PlayOpenMenuSound();
                break;
            default:
                menuSFX.PlayButtonClick();
                break;
        }
        menuSFX.PlayButtonClick();
    }
}
