using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    private int playerLayer;
    private EnemyMovement enemyMovement;
    private EnemyHealth enemyHealth;
    private EnemyAttack enemyAttack;
    void Awake()
    {
        playerLayer=LayerMask.NameToLayer("Player");
        enemyMovement=GetComponentInParent<EnemyMovement>();
        enemyHealth=GetComponentInParent<EnemyHealth>();
        enemyAttack=GetComponentInParent<EnemyAttack>();
    }
    void OnEnable()
    {
        enemyHealth.OnDeath+=EnemyDeath;
    }
    void OnDisable()
    {
        enemyHealth.OnDeath-=EnemyDeath;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==playerLayer && other.TryGetComponent<IDamageable>(out var damageable))
        {
            enemyMovement.PlayerEnteredAttackRange();
            enemyAttack.SetTarget(damageable);
            Debug.Log("attack player");
        }
    }
    void OnTriggerExit(Collider other)
    {
         if(other.gameObject.layer==playerLayer)
        {
            enemyMovement.PlayerLeftAttackRange();
            enemyAttack.ClearTarget();
            Debug.Log("player out of reach");
        }
    }
    void EnemyDeath()
    {
        gameObject.SetActive(false);
    }
}
