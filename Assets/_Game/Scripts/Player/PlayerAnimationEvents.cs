using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private PlayerShadowInteraction playerShadowInteraction;

    public void ReleaseWeapon()
    {
        Debug.Log("=== THROW EVENT FIRED ===");

        playerShadowInteraction.ReleaseWeapon();
    }
}