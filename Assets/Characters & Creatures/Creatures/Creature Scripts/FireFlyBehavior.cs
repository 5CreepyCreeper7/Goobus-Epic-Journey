using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireFlyBehavior : MonoBehaviour
{
    private Light2D fireflyLight;

    // Glow variables.
    public float maxLightIntensity = .4f;
    public float minLightIntensity = .2f;
    public float lightChangeSpeed = 1f;

    public float maxOuterRadius = 1f;
    public float minOuterRadius = .8f;

    // Movement variables.
    public float moveableRadius = 3f;
    private float currentMoveSpeed;
    public float targetReachedDistance = .1f;
    public float minWaitTime = 0.2f;
    public float maxWaitTime = 1f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool waiting = false;
    private float waitTimer = 0f;

    // Randomization variables.
    public float flightWobbleAmount = 0.15f;
    public float wobbleSpeed = 2f;
    private float wobbleOffsetX;
    private float wobbleOffsetY;

    public float minMoveSpeed = 0.5f;
    public float maxMoveSpeed = 1.5f;

    private float glowOffset;

    
    

    void Awake()
    {
        fireflyLight = GetComponentInChildren<Light2D>();
    }

    void Start() {
        startPosition = transform.position;

        glowOffset = Random.Range(0f, 100f);
        wobbleOffsetX = Random.Range(0f, 100f);
        wobbleOffsetY = Random.Range(0f, 100f);

        currentMoveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

        newTarget();
        waitTimer = Random.Range(minWaitTime, maxWaitTime);
    }

    // Update is called once per frame
    void Update()
    {
        updateGlow();   
        updatePosition();
    }

    private void updateGlow() {
        float time = (Mathf.Sin((Time.time + glowOffset)* lightChangeSpeed) + 1f) / 2f;

        fireflyLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, time);
        fireflyLight.pointLightOuterRadius = Mathf.Lerp(minOuterRadius, maxOuterRadius, time);
    }

    private void updatePosition() {
        if(waiting) {
            waitTimer -= Time.deltaTime;
            if(waitTimer <= 0f) {
                waiting = false;
                currentMoveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
                newTarget();
            }

            return;
        }

        Vector3 nextPosition = Vector3.MoveTowards(transform.position, targetPosition, currentMoveSpeed * Time.deltaTime);

        float wobbleX = (Mathf.PerlinNoise(Time.time * wobbleSpeed, wobbleOffsetX) - 0.5f) * 2f * flightWobbleAmount;
        float wobbleY = (Mathf.PerlinNoise(Time.time * wobbleSpeed, wobbleOffsetY) - 0.5f) * 2f * flightWobbleAmount;

        transform.position = nextPosition + new Vector3(wobbleX, wobbleY, 0f) * 0.1f;

        if(Vector3.Distance(transform.position, targetPosition) <= targetReachedDistance) {
            waiting = true;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    void newTarget() {
        Vector2 randomOffset = Random.insideUnitCircle * moveableRadius;
        targetPosition = startPosition + new Vector3(randomOffset.x, randomOffset.y, 0f);
    }
}
