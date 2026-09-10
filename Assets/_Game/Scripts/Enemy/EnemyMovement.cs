using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minimumDistance = 0.1f;

    private EnemyHealth enemyHealth;
    private Transform currentTarget;
    private Transform detectedPlayer;

    private bool playerInAttackRange;

    private enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    private EnemyState currentState;

    public bool CanAttack =>
        currentState == EnemyState.Attack &&
        detectedPlayer != null;
    public Vector3 FacingDirection { get; private set; } = Vector3.right;


    void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();

        currentState = EnemyState.Patrol;
        currentTarget = pointA;
    }


    void OnEnable()
    {
        enemyHealth.OnDeath += EnemyDeath;
    }


    void OnDisable()
    {
        enemyHealth.OnDeath -= EnemyDeath;
    }


    void Update()
    {
        if (currentState == EnemyState.Chase)
        {
            ChasePlayer();
        }
        else if (currentState == EnemyState.Patrol)
        {
            EnemyPatrol();
        }
    }


    private void EnemyPatrol()
    {
        if (currentTarget == null)
        {
            return;
        }

        if (Vector3.Distance(transform.position, currentTarget.position)
            >= minimumDistance)
        {
            Vector3 direction =
                currentTarget.position - transform.position;

            if (Mathf.Abs(direction.x) > 0.01f)
            {
                FacingDirection =
                    direction.x > 0f
                    ? Vector3.right
                    : Vector3.left;
            }
            transform.position = Vector3.MoveTowards(
                transform.position,
                currentTarget.position,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            if (currentTarget == pointA)
            {
                currentTarget = pointB;
            }
            else
            {
                currentTarget = pointA;
            }
        }
    }


    private void ChasePlayer()
    {
        if (detectedPlayer == null)
        {
            return;
        }
        Vector3 direction =
            detectedPlayer.position - transform.position;

        if (Mathf.Abs(direction.x) > 0.01f)
        {
            FacingDirection =
                direction.x > 0f
                ? Vector3.right
                : Vector3.left;
        }
        transform.position = Vector3.MoveTowards(
            transform.position,
            detectedPlayer.position,
            moveSpeed * Time.deltaTime
        );
    }


    public void PlayerDetected(Transform player)
    {
        detectedPlayer = player;

        if (playerInAttackRange)
        {
            currentState = EnemyState.Attack;
        }
        else
        {
            currentState = EnemyState.Chase;
        }

        Debug.Log(
            "Player detected | AttackRange: "
            + playerInAttackRange
            + " | State: "
            + currentState
        );
    }


    public void PlayerLost()
    {
        detectedPlayer = null;
        currentState = EnemyState.Patrol;

        Debug.Log("Player lost | State: Patrol");
    }


    public void PlayerEnteredAttackRange()
    {
        playerInAttackRange = true;

        Debug.Log("ENTERED ATTACK RANGE");

        if (detectedPlayer != null)
        {
            currentState = EnemyState.Attack;
        }
    }


    public void PlayerLeftAttackRange()
    {
        playerInAttackRange = false;

        Debug.Log("LEFT ATTACK RANGE");

        if (detectedPlayer != null)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }


    private void EnemyDeath()
    {
        enabled = false;
    }
}