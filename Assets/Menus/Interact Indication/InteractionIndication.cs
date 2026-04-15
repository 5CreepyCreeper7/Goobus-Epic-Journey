using UnityEngine;
using TMPro;

public class InteractionIndication : MonoBehaviour
{
    public GameObject interactionIndicatorPrefab;
    private GameObject currentIndicator;

    public float offsetY = 4;

    public float offsetX = 0;

    private void Update() {
        bobbingAnimation();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            if(currentIndicator == null) {
                Vector3 SpawnPoint = transform.position + new Vector3(offsetX, offsetY, 0);

                currentIndicator = Instantiate(interactionIndicatorPrefab, SpawnPoint, Quaternion.identity);

                setText(currentIndicator);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            if(currentIndicator != null) {
                Destroy(currentIndicator);
                currentIndicator = null;
            }
        }
    }

    private void setText(GameObject indicator) {
        TextMeshPro text = indicator.GetComponentInChildren<TextMeshPro>();

        if(text == null) {
            Debug.LogError("No TextMeshPro component found in children of the interaction indicator prefab.");
            return;
        }

        switch(gameObject.tag) {
            case "RecordPlayer":
                text.text = "Listen";
                break;
            case "NPC":
                text.text = "Talk";
                break;
            case "Sign":
                text.text = "Read";
                break;
            default:
                text.text = "Inspect";
                break;
        }
    }

    private void bobbingAnimation() {
        if(currentIndicator != null) {
            float bobbingSpeed = 2f;
            float bobbingAmount = 0.1f;

            Vector3 originalPosition = transform.position + new Vector3(offsetX, offsetY, 0);
            currentIndicator.transform.position = originalPosition + new Vector3(0, Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount, 0);
        }
    }

    public void HideIndicator() {
        if(currentIndicator != null) {
            currentIndicator.SetActive(false);
        }
    }

    public void ShowIndicator() {
        if(currentIndicator != null) {
            currentIndicator.SetActive(true);
        }
    }
}
