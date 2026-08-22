using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BossFightManager : MonoBehaviour
{
    private bool fightStarted;
    private bool fightEnded;
    private MusicManager musicManager;

    [Header("Boss")]
    [SerializeField] private GameObject boss;
    [SerializeField] private EnemyStats bossStats;
    [SerializeField] private EggBossBehavior bossBehavior;

    [Header("Boss Arena")]
    [SerializeField] private GameObject Entrance;
    [SerializeField] private GameObject Exit;

    [Header("Timing Settings")]
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float endDelay = 1f;

    [Header("Events")]
    [SerializeField] private UnityEvent onFightStart;
    [SerializeField] private UnityEvent onFightEnd;

    [Header("Boss Music")]
    [SerializeField] private float musicFadeDuration;
    [SerializeField] private AudioClip MainLevelMusic;
    [SerializeField] private AudioClip MinorBossMusic;
    [SerializeField] private AudioClip ChessBossMusic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (bossBehavior != null) {
            bossBehavior.enabled = false;
        }

        if (Entrance != null) {
            Entrance.SetActive(false);
        }

        if (Exit != null) {
            Exit.SetActive(true);
        }

        musicManager = MusicManager.Instance;

        if(musicManager == null)
        {
            Debug.LogError("Boss fight manager could not find music manager.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!fightStarted || fightEnded || bossStats == null) {
            return;
        }

        if(bossStats.enemyHealth <= 0) {
            EndFight();
        }
    }

    public void BeginBossFight() {
        if(fightStarted || fightEnded) {
            return;
        }

        fightStarted = true;

        if (musicManager != null) {
            MainLevelMusic = musicManager.CurrentClip;
        }
        
        StartCoroutine(BeginFightRoutine());
    }

    private IEnumerator BeginFightRoutine() {
        if(Entrance != null) {
            Entrance.SetActive(true);
        }

        //Play Goobus giving gift sequence if correct gift exit routine, if not begin fight

        yield return new WaitForSeconds(startDelay);

        if(boss != null && boss.activeInHierarchy) {
            boss.SetActive(true);
        }

        //play boss fight intro animation, music and effects
        StartMinorBossMusic();

        yield return new WaitForSeconds(startDelay);

        if(bossBehavior != null) {
            bossBehavior.enabled = true;
        }

        onFightStart?.Invoke();
    }

    public void EndFight() {
        if(!fightStarted || fightEnded) {
            return;
        }

        fightEnded = true;

        if (bossBehavior != null) {
            bossBehavior.enabled = false;
        }

        StartCoroutine(EndFightCoroutine());
    }

    private IEnumerator EndFightCoroutine() {
        // After fight Dialogue, rewards, and victory things
        yield return new WaitForSeconds(endDelay);

        if(Entrance != null) {
            Entrance.SetActive(false);
        }

        if(Exit != null) {
            Exit.SetActive(false);
        }

        StartMainLevelMusic();
        
        onFightEnd?.Invoke();   
    }

    public void StartMinorBossMusic() {
        if(musicManager == null || MinorBossMusic == null) {
            return;
        }

        musicManager.StartMusicWithFade(MinorBossMusic, musicFadeDuration);
    }

    public void StartMainLevelMusic() {
        if(musicManager == null || MainLevelMusic == null) {
            return;
        }

        musicManager.StartMusicWithFade(MainLevelMusic, musicFadeDuration);
    }
}
