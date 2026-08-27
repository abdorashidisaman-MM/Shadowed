using UnityEngine;

public class EnemyShadow : MonoBehaviour , IDamageable
{
    private BoxCollider enemyShadowCollider;
    private EnemyHealth enemyHumanHealth;
    void Awake()
    {
        enemyHumanHealth=GetComponentInParent<EnemyHealth>();
        enemyShadowCollider=GetComponent<BoxCollider>();
    

    }
     void OnEnable()
    {
        enemyHumanHealth.OnDeath+=DeathHandler;
    }
    void OnDisable()
    {
        enemyHumanHealth.OnDeath-=DeathHandler;
    }
    private void DeathHandler()
    {
        enemyShadowCollider.enabled=false;
        Debug.Log("i heard that the enemy is dead");
    }
    
    public void TakeDamage(float amount)
        {
            enemyHumanHealth.TakeDamage(amount);
        }
}

