using UnityEngine;
using System.Collections;

public class MusicSwapper : MonoBehaviour
{
    public AudioSource ChillMusicSource;
    public AudioSource IntenseMusicSource;

    private float transitionDuration = 2.0f;

    private Coroutine transitionCoroutine;
    private bool isAngry;

    void Start()
    {
        StartSynced();
    }

    private void StartSynced()
    {
        ChillMusicSource.loop = true;
        IntenseMusicSource.loop = true;

        ChillMusicSource.volume = 1.0f;
        IntenseMusicSource.volume = 0.0f;

        double startTime = AudioSettings.dspTime + 0.1f;

        ChillMusicSource.PlayScheduled(startTime);
        IntenseMusicSource.PlayScheduled(startTime);
    }

    public void SetAngryState(bool angry)
    {
        if (isAngry != angry)
        {
            isAngry = angry;

            if (transitionCoroutine != null)
            {
                StopCoroutine(transitionCoroutine);
            }

            transitionCoroutine = StartCoroutine(Crossfade(angry));
        }
    }

    private IEnumerator Crossfade(bool toIntense) {
        float elapsedTime = 0f;

        float calmStartVolume = ChillMusicSource.volume;
        float intenseStartVolume = IntenseMusicSource.volume;

        float calmTargetVolume = toIntense ? 0f : 1f;
        float intenseTargetVolume = toIntense ? 1f : 0f;

        while(elapsedTime < transitionDuration) {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            float curvedT = Mathf.Sin(t * Mathf.PI * 0.5f);

            ChillMusicSource.volume = Mathf.Lerp(calmStartVolume, calmTargetVolume, curvedT);
            IntenseMusicSource.volume = Mathf.Lerp(intenseStartVolume, intenseTargetVolume, curvedT);

            yield return null;
        }

        ChillMusicSource.volume = calmTargetVolume;
        IntenseMusicSource.volume = intenseTargetVolume;
        transitionCoroutine = null;
    }
}
