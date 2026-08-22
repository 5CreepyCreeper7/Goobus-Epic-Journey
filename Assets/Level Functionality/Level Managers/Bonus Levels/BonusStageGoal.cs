using UnityEngine;

public class BonusStageGoal : MonoBehaviour, IInteractable
{
    private bool used;

    public void Interact()
    {
        if(used || BonusRoomManager.Instance == null || RoomManager.Instance.InTransition)
        {
            return;
        }

        used = true;

        BonusRoomManager.Instance.CompleteBonusStage();
    }
}