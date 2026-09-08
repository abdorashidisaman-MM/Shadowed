using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    private EnemyHealth enemyHealth;

    private int playerLayerIndex;

    private PlayerStealth playerStealth;
    private Transform detectedPlayer;

    private bool isPlayerDetected;

    [SerializeField] private LayerMask visionMask;
    [SerializeField] private Transform visionOrigin;


    void Awake()
    {
        playerLayerIndex = LayerMask.NameToLayer("Player");

        enemyMovement = GetComponentInParent<EnemyMovement>();
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }


    void Update()
    {
        if (detectedPlayer == null || playerStealth == null)
        {
            return;
        }

        EvaluateDetection();
    }


    void OnEnable()
    {
        enemyHealth.OnDeath += DisableDetection;
    }


    void OnDisable()
    {
        enemyHealth.OnDeath -= DisableDetection;

        if (playerStealth != null)
        {
            playerStealth.OnStealthChanged -= HandleStealthChanged;
        }
    }


    void OnTriggerEnter(Collider other)
{
    if (other.gameObject.layer != playerLayerIndex)
    {
        return;
    }

    detectedPlayer = other.transform;
    playerStealth = other.GetComponentInParent<PlayerStealth>();

    if (playerStealth == null)
    {
        return;
    }

    playerStealth.OnStealthChanged += HandleStealthChanged;

    EvaluateDetection();
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayerIndex)
        {
            return;
        }

        if (playerStealth != null)
        {
            playerStealth.OnStealthChanged -= HandleStealthChanged;
        }

        if (isPlayerDetected)
        {
            enemyMovement.PlayerLost();
        }

        isPlayerDetected = false;

        detectedPlayer = null;
        playerStealth = null;
    }


    private void HandleStealthChanged(bool isHidden)
    {
        EvaluateDetection();
    }


    private void EvaluateDetection()
    {
        if (detectedPlayer == null || playerStealth == null)
        {
            return;
        }

        bool shouldDetect =
            !playerStealth.IsHidden &&
            CanSeePlayer();

        if (shouldDetect == isPlayerDetected)
        {
            return;
        }

        if (shouldDetect)
        {
            enemyMovement.PlayerDetected(detectedPlayer);
        }
        else
        {
            enemyMovement.PlayerLost();
        }

        isPlayerDetected = shouldDetect;
    }


   private bool CanSeePlayer()
{
    Vector3 directionToPlayer =
        detectedPlayer.position - visionOrigin.position;

    float distanceToPlayer = directionToPlayer.magnitude;

    RaycastHit hit;

    if (Physics.Raycast(
        visionOrigin.position,
        directionToPlayer.normalized,
        out hit,
        distanceToPlayer,
        visionMask,
        QueryTriggerInteraction.Ignore))
    {
        Debug.Log(
            "VISION HIT: " +
            hit.collider.name +
            " | Layer: " +
            LayerMask.LayerToName(hit.collider.gameObject.layer)
        );

        PlayerStealth hitPlayer =
            hit.collider.GetComponentInParent<PlayerStealth>();

        if (hitPlayer == playerStealth)
        {
            Debug.DrawRay(
                visionOrigin.position,
                directionToPlayer,
                Color.green
            );

            return true;
        }

        Debug.DrawRay(
            visionOrigin.position,
            directionToPlayer,
            Color.red
        );

        return false;
    }

    Debug.Log("VISION HIT NOTHING");

    Debug.DrawRay(
        visionOrigin.position,
        directionToPlayer,
        Color.yellow
    );

    return false;
}


    private void DisableDetection()
    {
        gameObject.SetActive(false);
    }
}