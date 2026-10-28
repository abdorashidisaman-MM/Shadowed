using UnityEngine;
using UnityEngine.Playables;

public class LastWeapon : MonoBehaviour
{
  
   [SerializeField] private GameObject lastWeapon;
   [SerializeField] private PlayerShadowInteraction shadowInteraction;
   public void LastWeaponComes()
    {
        shadowInteraction.NormalAttackEnabler();

        // The final weapon is hidden until this moment.
        if (lastWeapon != null)
        {
            lastWeapon.SetActive(true);
        }

       
        

    }
}
