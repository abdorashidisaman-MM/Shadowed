using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private PlayerShadowInteraction playerShadowInteraction;

    // Animation Event: Player_WeaponThrow release frame.
    public void ReleaseWeapon()
    {
        if (playerShadowInteraction != null)
        {
            playerShadowInteraction.ReleaseWeapon();
        }
    }

    // Animation Event: Player_WeaponTake, frame where the hand reaches the shadow.
    public void GrabContact()
    {
        if (playerShadowInteraction != null)
        {
            playerShadowInteraction.OnGrabContact();
        }
    }
}