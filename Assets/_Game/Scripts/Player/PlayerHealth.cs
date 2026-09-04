using UnityEngine;

public class PlayerHealth : MonoBehaviour,IDamageable
{
    [SerializeField] private float maximumHealth=100f;
    private float currentHealth;
    public event System.Action OnDeath;
    private bool isDead;
    
    void Awake()
    {
        currentHealth=maximumHealth;
    }
   public void TakeDamage(float amount)
    {
        if(!isDead)
        {
            currentHealth-=amount;
            Debug.Log("Player have "+currentHealth +" health left");
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
    }
}
