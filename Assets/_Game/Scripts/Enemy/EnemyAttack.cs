using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackCooldown = 1f;

        private float attackTimer;
        private EnemyHealth enemyHealth; 
        private IDamageable target;
    void Awake()
    {
        enemyHealth=GetComponentInParent<EnemyHealth>();
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
            attackTimer -= Time.deltaTime;

            if (target != null && attackTimer <= 0f)
            {
                target.TakeDamage(attackDamage);
                attackTimer = attackCooldown;
            }
        }
        public void SetTarget(IDamageable player)
    {
        target=player;
    }
    public void ClearTarget()
    {
        target=null;
    }
    void EnemyDeath()
    {
        enabled=false;
    }
}
