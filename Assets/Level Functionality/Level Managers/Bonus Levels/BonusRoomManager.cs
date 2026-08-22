using UnityEngine;

public class BonusRoomManager : MonoBehaviour
{
    public static BonusRoomManager Instance { get; private set; }
    [SerializeField] private AudioClip BonusLevelMusic;
    [SerializeField] private Shader BonusLevelShader;
    private bool inBonusStage;
    private bool stageEnded;

    public bool InBonusStage => inBonusStage;
    public float fadeTime = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void playBonusLevelMusic()
    {
        if(MusicManager.Instance == null)
        {
            return;
        }

        MusicManager.Instance.StartMusicWithFade(BonusLevelMusic, fadeTime);
    }
    
    public void BeginBonusStage(string bonusScene, int bonusSpawnPointID, int returnSpawnPointID)
    {
        if(inBonusStage || RoomManager.Instance.InTransition)
        {
            return;
        }

        inBonusStage = true;
        stageEnded = false;

        playBonusLevelMusic();

        RoomManager.Instance.EnterBonusStage(bonusScene, bonusSpawnPointID, returnSpawnPointID);
    }

    public void TransitionToBonusRoom(string sceneName, int spawnPointID)
    {
        if(!inBonusStage || RoomManager.Instance.InTransition)
        {
            return;
        }

        RoomManager.Instance.TransitionToRoom(sceneName, spawnPointID);
    }

    public void CompleteBonusStage()
    {
        if(!inBonusStage || stageEnded)
        {
            return;
        }

        stageEnded = true;

        //Reward

        ExitBonusStage();
    }

    public void FailBonusStage()
    {
        if(!inBonusStage || stageEnded)
        {
            return;
        }

        stageEnded = true;
        ExitBonusStage();
    }

    private void ExitBonusStage()
    {
        inBonusStage = false;
        RoomManager.Instance.ExitBonusStage();
    }


}
