using UnityEngine;

public class ProjectileHitbox : MonoBehaviour
{
    private ShadowRange fatherShadow;
    private int playerLayerIndex;
    private void Awake()
    {
        fatherShadow = GetComponentInParent<ShadowRange>();
        playerLayerIndex = LayerMask.NameToLayer("Player");
    }
    void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.layer !=playerLayerIndex)
        {
        fatherShadow.HandleProjectileHit(other);
        }    
    }

}
