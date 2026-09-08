using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
   private EnemyMovement enemyMovement;
   private EnemyHealth enemyHealth;
    private int playerLayerIndex;
    private PlayerStealth playerStealth;
    private Transform detectedPlayer;
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
        if (playerStealth != null)
            {
                playerStealth.OnStealthChanged -= HandleStealthChanged;
            }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==playerLayerIndex)
        {
            detectedPlayer=other.transform;
            playerStealth=other.GetComponentInParent<PlayerStealth>();
            playerStealth.OnStealthChanged += HandleStealthChanged;
            HandleStealthChanged(playerStealth.IsHidden);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer==playerLayerIndex)
        {
            if (playerStealth != null)
            {
                playerStealth.OnStealthChanged -= HandleStealthChanged;
            }

            enemyMovement.PlayerLost();
            detectedPlayer = null;
            playerStealth = null;
        }
    }
    void HandleStealthChanged(bool isHidden)
    {
        if(isHidden)
        {
            enemyMovement.PlayerLost();
        }
        else
        {
            enemyMovement.PlayerDetected(detectedPlayer);
        }
    }
    private void DisableDetection()
{
    gameObject.SetActive(false);
}
}
