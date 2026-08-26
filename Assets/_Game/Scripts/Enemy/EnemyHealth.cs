using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private  bool isDead;
    private float enemyHealth=100f;
    public System.Action OnDeath;

    public void TakeDamage(float amount)
    {
        if(!isDead)
        {
        enemyHealth-=amount;
        Debug.Log("Current Damage : "+enemyHealth);
        if(enemyHealth<=0)
        {
        Die();
        }
        }
    }
    private void Die()
    {
         
            
                isDead=true;
                OnDeath?.Invoke();
                Debug.Log("enemy is dead"); 
            
    }
    
}
