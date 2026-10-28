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
        if (fatherShadow == null || !fatherShadow.IsThrown)
            return;

        // The shadow decides whether this contact counts (and stops
        // being a projectile), so damage is applied at most once.
        if (fatherShadow.HandleProjectileHit(other) &&
            other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(attackDamage);
        }
    }



}
