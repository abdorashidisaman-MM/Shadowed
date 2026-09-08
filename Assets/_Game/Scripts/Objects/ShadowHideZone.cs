using UnityEngine;

public class ShadowHideZone : MonoBehaviour
{
    private int playerLayer;
    private PlayerStealth playerStealth;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            playerStealth = other.GetComponentInParent<PlayerStealth>();

            if (playerStealth != null)
            {
                playerStealth.EnterShadow(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == playerLayer && playerStealth != null)
        {
            playerStealth.ExitShadow(this);
            playerStealth = null;
        }
    }
}