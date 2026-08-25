using UnityEngine;
using System.Collections;

public class BonusLevelShaderController : MonoBehaviour
{
    public static BonusLevelShaderController Instance { get; private set; }

    [SerializeField] private Material dreamShaderMaterial;
    [SerializeField] private float defaultRampDuration = 1.5f;

    private static readonly int WaveIntensityID = Shader.PropertyToID("_WaveIntensity");
    private static readonly int HueIntensityID = Shader.PropertyToID("_HueIntensity");
    private static readonly int AberrationIntensityID = Shader.PropertyToID("_AberrationIntensity");

    private Coroutine waveRamp;
    private Coroutine hueRamp;
    private Coroutine aberrationRamp;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RampWaveTo(float target, float? duration = null)
    {
        if (waveRamp != null) StopCoroutine(waveRamp);
        waveRamp = StartCoroutine(RampProperty(WaveIntensityID, target, duration ?? defaultRampDuration, r => waveRamp = r));
    }

    public void RampHueTo(float target, float? duration = null)
    {
        if (hueRamp != null) StopCoroutine(hueRamp);
        hueRamp = StartCoroutine(RampProperty(HueIntensityID, target, duration ?? defaultRampDuration, r => hueRamp = r));
    }

    public void RampAberrationTo(float target, float? duration = null)
    {
        if (aberrationRamp != null) StopCoroutine(aberrationRamp);
        aberrationRamp = StartCoroutine(RampProperty(AberrationIntensityID, target, duration ?? defaultRampDuration, r => aberrationRamp = r));
    }

    // ramps all three together
    public void RampTo(float target, float? duration = null)
    {
        RampWaveTo(target, duration);
        RampHueTo(target, duration);
        RampAberrationTo(target, duration);
    }

    private IEnumerator RampProperty(int propertyID, float target, float duration, System.Action<Coroutine> onComplete)
    {
        float start = dreamShaderMaterial.GetFloat(propertyID);
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            dreamShaderMaterial.SetFloat(propertyID, Mathf.Lerp(start, target, t));
            yield return null;
        }

        dreamShaderMaterial.SetFloat(propertyID, target);
        onComplete(null);
    }
}