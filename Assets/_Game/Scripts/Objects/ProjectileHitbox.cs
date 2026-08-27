using UnityEngine;

public class ProjectileHitbox : MonoBehaviour
{
    [SerializeField] private float attackDamage = 10f;
    private ShadowRange fatherShadow;   
    private void Awake()
    {
        fatherShadow = GetComponentInParent<ShadowRange>();
        
    }
    void OnTriggerEnter(Collider other)
    {
        
       
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(attackDamage);
                
            }
            fatherShadow.HandleProjectileHit(other);
           
    }



}
