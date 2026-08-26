using UnityEngine;

public class EnemyShadow : MonoBehaviour
{
    private int projectileLayer;
    private BoxCollider enemyShadowCollider;
    private EnemyHealth enemyHealth;
    void Awake()
    {
        enemyHealth=GetComponentInParent<EnemyHealth>();
        projectileLayer=LayerMask.NameToLayer("Projectile");
        enemyShadowCollider=GetComponent<BoxCollider>();
    

    }
     void OnEnable()
    {
        enemyHealth.OnDeath+=DeathHandler;
    }
    void OnDisable()
    {
        enemyHealth.OnDeath-=DeathHandler;
    }
    private void DeathHandler()
    {
        enemyShadowCollider.enabled=false;
        Debug.Log("i heard that the enemy is dead");
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.layer==projectileLayer)
        {
            if (other.TryGetComponent<ProjectileHitbox>(out var projectile))
             {
            enemyHealth.TakeDamage(projectile.attackDamage);
             }
        }
    }
}

