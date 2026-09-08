using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    private int playerLayer;

    private EnemyMovement enemyMovement;
    private EnemyHealth enemyHealth;
    private EnemyAttack enemyAttack;


    void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");

        enemyMovement = GetComponentInParent<EnemyMovement>();
        enemyHealth = GetComponentInParent<EnemyHealth>();
        enemyAttack = GetComponentInParent<EnemyAttack>();
    }


    void OnEnable()
    {
        enemyHealth.OnDeath += EnemyDeath;
    }


    void OnDisable()
    {
        enemyHealth.OnDeath -= EnemyDeath;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != playerLayer)
        {
            return;
        }

        IDamageable damageable =
            other.GetComponentInParent<IDamageable>();

        if (damageable == null)
        {
            return;
        }

        enemyMovement.PlayerEnteredAttackRange();
        enemyAttack.SetTarget(damageable);

        Debug.Log("Attack target acquired");
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayer)
        {
            return;
        }

        enemyMovement.PlayerLeftAttackRange();
        enemyAttack.ClearTarget();

        Debug.Log("Player out of attack range");
    }


    private void EnemyDeath()
    {
        gameObject.SetActive(false);
    }
}