using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed=5f;
    [SerializeField] private float minimumDistance=0.1f;
    private EnemyHealth enemyHealth;
    private Transform currentTarget;
    private enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }
    private EnemyState currentState;
    private Transform detectedPlayer;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        enemyHealth=GetComponent<EnemyHealth>();
        currentState=EnemyState.Patrol;
        currentTarget=pointA;

    }

    void OnEnable()
    {
        enemyHealth.OnDeath+=EnemyDeath;
    }
    void OnDisable()
    {
        enemyHealth.OnDeath-=EnemyDeath;
    }
    void Update()
    {
        if(currentState==EnemyState.Chase)
        {
            ChasePlayer();
        }
        else if(currentState==EnemyState.Patrol)
        {
        EnemyPatrol();
        }
    }
    void EnemyPatrol()
    {
        if(Vector3.Distance(transform.position, currentTarget.position)>=minimumDistance)
        {
        transform.position=Vector3.MoveTowards(transform.position,currentTarget.position,moveSpeed*Time.deltaTime);
        }
        else
        {
            if(currentTarget==pointA)
            {
            currentTarget=pointB;
            }
            else
            {
                currentTarget=pointA;
            }
        }
    }
    void EnemyDeath()
    {
        enabled = false;
    }
   public void PlayerDetected(Transform player)
    {
       detectedPlayer=player;
       currentState=EnemyState.Chase;
       
    }
   public void PlayerLost()
    {
        detectedPlayer=null;
        currentState=EnemyState.Patrol;
     
    }
    private void ChasePlayer()
    {
         transform.position=Vector3.MoveTowards(transform.position,detectedPlayer.position,moveSpeed*Time.deltaTime);
    }
    public void PlayerEnteredAttackRange()
    {
        currentState = EnemyState.Attack;
    }
    public void PlayerLeftAttackRange()
    {
        if (detectedPlayer != null)
        {
            currentState = EnemyState.Chase;
        }
    }
}
