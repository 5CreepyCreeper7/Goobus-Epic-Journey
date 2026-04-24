using UnityEngine;

public class PlaceCharacter : MonoBehaviour
{
    private GameObject PlacementPoint;
    private GameObject GroundCheck;

    void Start()
    {
        PlacementPoint = GameObject.FindGameObjectWithTag("PlayerPlacement");
        GroundCheck = GameObject.FindGameObjectWithTag("GroundCheck");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAcceptablePoint();
    }

    private void UpdateAcceptablePoint() {
        if(PlacementPoint != null && GroundCheck != null && GetComponent<PlayerMovement>().isGrounded) {
            PlacementPoint.transform.position = GroundCheck.transform.position;
        }
    }

    public void PlacePlayer() {
        if(PlacementPoint != null) {
            transform.position = PlacementPoint.transform.position;
        }
    }
}
