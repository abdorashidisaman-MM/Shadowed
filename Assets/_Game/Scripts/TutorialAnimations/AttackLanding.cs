using UnityEngine;
using UnityEngine.Playables;

public class AttackLanding : MonoBehaviour
{
    [SerializeField] private PlayerShadowInteraction playerInteraction;
    [SerializeField] private PlayableDirector landingDirector;
    [SerializeField] private GameObject targetEnemyShadow;
    [SerializeField] private float landingDamage = 100f;
    [SerializeField] private ShadowSurface landingSurface;
    [SerializeField] private ShadowSurfaceMotor shadowSurfaceMotor;

    private void OnEnable()
    {
        playerInteraction.OnShadowEquipped += StartLandingAttack;
    }

    private void OnDisable()
    {
        playerInteraction.OnShadowEquipped -= StartLandingAttack;
    }

    private void StartLandingAttack(ShadowRange weapon)
    {
        landingDirector.Play();
    }
    public void LandingImpact()
    {
        Debug.Log("Landing Impact");
        targetEnemyShadow.GetComponent<EnemyShadow>().TakeDamage(landingDamage);
        targetEnemyShadow.SetActive(false);
    }
    public void LandingFinished()
    {
        
        shadowSurfaceMotor.AttachToSurface(landingSurface);
        shadowSurfaceMotor.enabled=true;
        
    }
}
