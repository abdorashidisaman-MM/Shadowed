using Shadowed.Input;
using UnityEngine;
using UnityEngine.Playables;

public class AttackLanding : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerShadowInteraction playerInteraction;
    [SerializeField] private PlayerInputReader playerInputReader;
    [SerializeField] private ShadowSurfaceMotor shadowSurfaceMotor;

    [Header("Landing Attack")]
    [SerializeField] private PlayableDirector landingDirector;
    [SerializeField] private PlayableDirector secondSoldierDirector;
    [SerializeField] private EnemyShadow targetEnemyShadow;
    [SerializeField] private float landingDamage = 100f;
    [SerializeField] private float minimumAimScore = 0.7f;

    [Header("Landing")]
    [SerializeField] private ShadowSurface landingSurface;

    private bool landingAttackReady;


    private void OnEnable()
    {
        playerInteraction.OnShadowEquipped += SpecialAttackReady;
    }

    private void OnDisable()
    {
        playerInteraction.OnShadowEquipped -= SpecialAttackReady;
    }


    private void Update()
    {
        if (!landingAttackReady)
            return;

        // During this special state, AttackLanding owns the Attack button.
        if (!playerInputReader.ConsumeAttackRequest())
            return;


        Vector2 aimDirection =
            playerInputReader.AimInput.normalized;

        if (aimDirection == Vector2.zero)
            return;


        Vector3 directionToEnemy3D =
            targetEnemyShadow.transform.position
            - playerInteraction.transform.position;

        Vector2 directionToEnemy =
            new Vector2(
                directionToEnemy3D.x,
                directionToEnemy3D.y
            ).normalized;


        float aimScore =
            Vector2.Dot(
                aimDirection,
                directionToEnemy
            );


        if (aimScore < minimumAimScore)
            return;


        landingAttackReady = false;

        StartLandingAttack();
    }


    private void SpecialAttackReady(ShadowRange weapon)
    {
        landingAttackReady = true;
    }


    private void StartLandingAttack()
    {
        landingDirector.Play();
    }


    // Signal: LandingImpact (weapon contact frame).
    public void LandingImpact()
    {
        if (targetEnemyShadow != null)
        {
            targetEnemyShadow.TakeDamage(
                landingDamage
            );
        }
    }


    // Signal: LandingFinished.
    public void LandingFinished()
    {
        landingAttackReady = false;

        playerInteraction.ConsumeEquippedShadow();
        shadowSurfaceMotor.AttachToSurface(
            landingSurface
        );

        // One-shot sequence: stop listening for further equips.
        playerInteraction.OnShadowEquipped -= SpecialAttackReady;

        if (secondSoldierDirector != null)
        {
            secondSoldierDirector.Play();
        }
    }
}