using UnityEngine;

public class BellLightFollower : MonoBehaviour
{
    [SerializeField] private Transform bellLightAnchor;

    void LateUpdate()
    {
        if (PlayerMovement.Instance == null) return;

        Vector3 anchorPos = bellLightAnchor.localPosition;
        anchorPos.x = Mathf.Abs(anchorPos.x) * (PlayerMovement.Instance.IsFacingLeft ? 1f : -1f);
        bellLightAnchor.localPosition = anchorPos;
    }
}
