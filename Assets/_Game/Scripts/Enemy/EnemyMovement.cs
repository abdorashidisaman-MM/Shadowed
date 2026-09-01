using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed=5f;
    [SerializeField] private float minimumDistance=0.1f;
    private EnemyHealth enemyHealth;
    private Transform currentTarget;
    private Transform detectedPlayer;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        enemyHealth=GetComponent<EnemyHealth>();
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
        if(detectedPlayer!=null)
        {
            ChasePlayer();
        }
        else
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
       
    }
   public void PlayerLost()
    {
        detectedPlayer=null;
     
    }
    private void ChasePlayer()
    {
         transform.position=Vector3.MoveTowards(transform.position,detectedPlayer.position,moveSpeed*Time.deltaTime);
    }
}
