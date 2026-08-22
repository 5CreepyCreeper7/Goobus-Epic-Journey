using UnityEngine;

public class BonusStageTransition : MonoBehaviour
{
    [Header("BonusStage")]
    [SerializeField] private string bonusStageScene;
    [SerializeField] private int bonusStageSpawnPointID = 1;
    [SerializeField] private int returnSpawnPointID;


    public void transitionToBonusRoom()
    {
        if(RoomManager.Instance == null)
        {
            return;
        }
        
        RoomManager.Instance.TransitionToRoom(bonusStageScene, bonusStageSpawnPointID);
    }
}
