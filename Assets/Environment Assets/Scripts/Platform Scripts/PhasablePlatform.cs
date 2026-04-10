using UnityEngine;

public class PhasablePlatform : MonoBehaviour
{
    public Collider2D platformCollider;
    public PlayerMovement playerMovement;
    
    void Update() {
        phaseThrough();
    }

    void phaseThrough() {
        if(playerMovement.isCrouching) {
            platformCollider.isTrigger = true;
        } else {
            platformCollider.isTrigger = false;
        }
    }
}
