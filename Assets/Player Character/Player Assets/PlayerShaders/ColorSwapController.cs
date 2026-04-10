using UnityEngine;
using UnityEngine.UI;

public class ColorSwapController : MonoBehaviour
{
    public Color newColor;

    public bool usingSpriteRenderer = false;

    private Image imageComponent;
    private SpriteRenderer spriteRenderer;

    MaterialPropertyBlock propertyBlock;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(imageComponent == null && spriteRenderer == null) {
            Debug.LogError("No Image or SpriteRenderer component found on the GameObject.");
        }

        if(usingSpriteRenderer) {
            ChangeColorSpriteRenderer();
        } else {
            ChangeColorImage();
        }
    }

    public void ChangeColorImage() {
        Material mat = Instantiate(imageComponent.material);
        mat.CopyMatchingPropertiesFromMaterial(imageComponent.material);
        mat.SetColor("BellOut", newColor);
        imageComponent.material = mat;
    }

    public void ChangeColorSpriteRenderer() {
        propertyBlock = new();

        propertyBlock.SetColor("unity_SpriteColor", Color.white);
        propertyBlock.SetVector("unity_SpriteProps", new Vector4(1,1,-1,0));

        propertyBlock.SetColor("BellOut", newColor);

        spriteRenderer.SetPropertyBlock(propertyBlock);
    }
}
