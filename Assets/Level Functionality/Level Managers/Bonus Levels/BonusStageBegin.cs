using UnityEngine;

public class BonusStageBegin : MonoBehaviour, IInteractable
{
    [SerializeField] private string firstBonusRoomScene;
    [SerializeField] private int bonusSpawnPointID = 1;
    [SerializeField] private int returnSpawnPointID;
    private bool eatenBerries;

    public void Interact()
    {
        if(eatenBerries || RoomManager.Instance == null || RoomManager.Instance.InTransition)
        {
            return;
        }

        eatenBerries = true;

        BonusRoomManager.Instance.BeginBonusStage(firstBonusRoomScene, bonusSpawnPointID, returnSpawnPointID);
        
    }
}
