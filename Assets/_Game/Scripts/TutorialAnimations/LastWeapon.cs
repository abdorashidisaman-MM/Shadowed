using UnityEngine;
using UnityEngine.Playables;

public class LastWeapon : MonoBehaviour
{
   [SerializeField] private PlayableDirector lastWeaponAnimation;
   [SerializeField] private GameObject lastWeapon;
   [SerializeField] private PlayerShadowInteraction shadowInteraction;
   public void LastWeaponComes()
    {
        Debug.Log("LAST WEAPON COMES");
        shadowInteraction.NormalAttackEnabler();
        lastWeapon.SetActive(enabled);
        lastWeaponAnimation.Play();
        
        

    }
}
