using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
   private EnemyMovement enemyMovement;
   private EnemyHealth enemyHealth;
    private int playerLayerIndex;
    void Awake()
    {
        playerLayerIndex=LayerMask.NameToLayer("Player");
        enemyMovement=GetComponentInParent<EnemyMovement>();
        enemyHealth = GetComponentInParent<EnemyHealth>();

    }
    void OnEnable()
    {
        enemyHealth.OnDeath+=DisableDetection;
    }
    void OnDisable()
    {
        enemyHealth.OnDeath-=DisableDetection;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==playerLayerIndex)
        {
            enemyMovement.PlayerDetected(other.transform);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer==playerLayerIndex)
        {
            enemyMovement.PlayerLost();
        }
    }
    private void DisableDetection()
{
    gameObject.SetActive(false);
}
}
