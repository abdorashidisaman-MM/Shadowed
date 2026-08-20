using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShadow : MonoBehaviour
{
    private int projectileLayer;
    private EnemyHealth EnemyHumanHealth;
    void Awake()
    {
        EnemyHumanHealth=GetComponentInParent<EnemyHealth>();
        projectileLayer=LayerMask.NameToLayer("Projectile");

    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.layer==projectileLayer)
        {
            EnemyHumanHealth.Damage();
        }
    }
}

