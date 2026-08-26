using System.Reflection.Metadata.Ecma335;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileHitbox : MonoBehaviour
{
    [SerializeField] public float attackDamage = 10f;
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
