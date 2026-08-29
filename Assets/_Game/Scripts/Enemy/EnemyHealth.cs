using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private  bool isDead;
    [SerializeField] float MaxHealth=100f;
    private float currentHealth;
    public event System.Action OnDeath;
    void Awake()
    {
        currentHealth=MaxHealth;
    }
    public void TakeDamage(float amount)
    {
        if(!isDead)
        {
        currentHealth-=amount;
        Debug.Log("Current Damage : "+currentHealth);
        if(currentHealth<=0)
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
