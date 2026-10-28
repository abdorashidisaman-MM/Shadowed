using System;
using System.Collections.Generic;
using Shadowed.Input;
using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
public class PlayerShadowInteraction : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private float minimumTargetScore = 0.7f;

    [Header("Attack")]
    [SerializeField] private float attackSpeed = 10f;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform throwOrigin;

    [Header("Weapon Visual")]
    [SerializeField] private GameObject heldWeaponVisual;
    [SerializeField] private SpriteRenderer heldWeaponRenderer;

    [Header("Safety")]
    [Tooltip("If the grab-contact Animation Event does not arrive within this time, " +
             "the held weapon visual is shown anyway.")]
    [SerializeField] private float grabVisualFallbackDelay = 0.6f;
    [Tooltip("If the ReleaseWeapon Animation Event does not arrive within this time " +
             "(e.g. throw animation interrupted), the weapon is released anyway.")]
    [SerializeField] private float throwReleaseFallbackDelay = 1.0f;
    


    private readonly List<ShadowRange> nearbyShadows =
        new List<ShadowRange>();

    public IReadOnlyList<ShadowRange> NearbyShadows =>
        nearbyShadows;

    public event Action<ShadowRange> OnShadowEquipped;


    private PlayerInputReader inputReader;
    private PlayerHealth playerHealth;

    private Vector2 aimDirection;

    private float bestScore = -1f;
    private float bestDistance;

    private ShadowRange targetShadow;

    public ShadowRange equippedShadow
    {
        get;
        private set;
    }

    private Vector3 cachedAttackAim;

    private bool normalAttackAllowed;
    private bool throwInProgress;
    private float throwStartTime;

    // Grab: gameplay ownership changes on input, the visual swap
    // (world shadow -> hand) happens on the grab-contact frame.
    private bool heldVisualPending;
    private float equipTime;


    private static readonly int ThrowHash =
        Animator.StringToHash("Throw");

    private static readonly int GrabHash =
        Animator.StringToHash("Grab");


    private void Awake()
    {
        inputReader =
            GetComponent<PlayerInputReader>();

        playerHealth =
            GetComponent<PlayerHealth>();

        if (animator == null)
        {
            Debug.LogWarning(
                "PlayerShadowInteraction: no Animator assigned, grab/throw animations will not play.",
                this);
        }

        // Player starts with empty hands.
        if (heldWeaponVisual != null)
        {
            heldWeaponVisual.SetActive(false);
        }
    
    }


    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath +=
                PlayerDeath;
        }
    }


    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath -=
                PlayerDeath;
        }

        ClearTarget();
    }


    private void Update()
    {
        aimDirection =
            inputReader.AimInput.normalized;


        if (aimDirection == Vector2.zero)
        {
            ClearTarget();
        }
        else
        {
            FindBestTarget();
        }


        GrabHandle();
        AttackHandle();
        UpdateSafetyFallbacks();
    }


    private void UpdateSafetyFallbacks()
    {
        if (heldVisualPending &&
            Time.time - equipTime >= grabVisualFallbackDelay)
        {
            OnGrabContact();
        }

        if (throwInProgress &&
            Time.time - throwStartTime >= throwReleaseFallbackDelay)
        {
            ReleaseWeapon();
        }
    }


    // --------------------------------------------------
    // TARGETING
    // --------------------------------------------------

    private void FindBestTarget()
    {
        bestDistance =
            Mathf.Infinity;


        ShadowRange previousTarget =
            targetShadow;


        bestScore = -1f;
        targetShadow = null;


        // Drop stale entries (destroyed / disabled / already taken).
        nearbyShadows.RemoveAll(
            s => s == null || !s.isActiveAndEnabled || !s.IsGrabbable);


        foreach (ShadowRange shadow in nearbyShadows)
        {
            Vector2 toShadow =
                new Vector2(
                    shadow.transform.position.x -
                    transform.position.x,

                    shadow.transform.position.y -
                    transform.position.y
                );


            float distance =
                toShadow.magnitude;


            Vector2 directionToShadow =
                toShadow.normalized;


            float score =
                Vector2.Dot(
                    aimDirection,
                    directionToShadow
                );


            if (targetShadow == null)
            {
                bestScore =
                    score;

                bestDistance =
                    distance;

                targetShadow =
                    shadow;
            }
            else
            {
                float scoreDifference =
                    score - bestScore;


                if (scoreDifference > 0.01f)
                {
                    bestScore =
                        score;

                    bestDistance =
                        distance;

                    targetShadow =
                        shadow;
                }
                else if (
                    Mathf.Abs(scoreDifference) <= 0.01f &&
                    distance < bestDistance)
                {
                    bestScore =
                        score;

                    bestDistance =
                        distance;

                    targetShadow =
                        shadow;
                }
            }
        }


        if (bestScore < minimumTargetScore)
        {
            targetShadow =
                null;
        }


        if (previousTarget != targetShadow)
        {
            if (previousTarget != null)
            {
                previousTarget.SetTargeted(false);
            }

            if (targetShadow != null)
            {
                targetShadow.SetTargeted(true);
            }
        }
    }


    private void ClearTarget()
    {
        if (targetShadow != null)
        {
            targetShadow.SetTargeted(false);
        }


        bestScore = -1f;
        targetShadow = null;
    }


    public void AddShadow(
        ShadowRange shadow)
    {
        if (shadow == null)
            return;


        if (!nearbyShadows.Contains(shadow))
        {
            nearbyShadows.Add(shadow);
        }
    }


    public void RemoveShadow(
        ShadowRange shadow)
    {
        if (shadow == null)
            return;


        nearbyShadows.Remove(shadow);


        if (targetShadow == shadow)
        {
            ClearTarget();
        }
    }


    // --------------------------------------------------
    // GRAB
    // --------------------------------------------------

    private void GrabHandle()
    {
        // Only claim the Grab press when this system can actually use it.
        // Otherwise other Grab users (e.g. the ally hand) would randomly
        // lose the press depending on script execution order.
        if (targetShadow == null ||
            equippedShadow != null ||
            throwInProgress)
            return;


        if (!inputReader.ConsumeGrabRequest())
            return;


        EquipShadow(
            targetShadow
        );
    }


    private void EquipShadow(
        ShadowRange shadow)
    {
        if (equippedShadow != null || shadow == null)
            return;


        ShadowRange taken =
            shadow;

        ClearTarget();


        equippedShadow =
            taken;


        // Gameplay ownership changes immediately (no double grabs,
        // shadow stops being targetable).
        equippedShadow.Take(this);


        // Visual swap waits for the grab-contact frame of Player_WeaponTake.
        heldVisualPending = true;
        equipTime = Time.time;


        if (animator != null)
        {
            animator.SetTrigger(GrabHash);
        }


        OnShadowEquipped?.Invoke(
            equippedShadow
        );
    }


    // Animation Event (via PlayerAnimationEvents) on the frame the hand
    // reaches the shadow in Player_WeaponTake. Also used as fallback.
    public void OnGrabContact()
    {
        if (!heldVisualPending)
            return;

        heldVisualPending = false;

        if (equippedShadow != null)
        {
            equippedShadow.HideWorldVisual();

            if (heldWeaponVisual != null)
            {
                heldWeaponVisual.SetActive(true);
            }
        }
    }


    // --------------------------------------------------
    // ATTACK
    // --------------------------------------------------

private void AttackHandle()
{
    if (!normalAttackAllowed)
    {
        Debug.Log("ATTACK STOPPED: normalAttackAllowed false");
        return;
    }

    if (throwInProgress)
    {
        Debug.Log("ATTACK STOPPED: throw already in progress");
        return;
    }

    if (!inputReader.ConsumeAttackRequest())
        return;

    Debug.Log("=== ATTACK BUTTON PRESSED ===");

    if (equippedShadow == null)
    {
        Debug.Log("ATTACK STOPPED: no equipped weapon");
        return;
    }

    Debug.Log("Equipped weapon: " + equippedShadow.name);

    if (aimDirection == Vector2.zero)
    {
        Debug.Log("ATTACK STOPPED: aimDirection is ZERO");
        return;
    }

    Debug.Log("Aim: " + aimDirection);

    cachedAttackAim = new Vector3(
        aimDirection.x,
        aimDirection.y,
        0f
    );

    OnGrabContact();

    throwInProgress = true;
    throwStartTime = Time.time;

    Debug.Log("THROW TRIGGER SENT");

    if (animator != null)
    {
        animator.SetTrigger(ThrowHash);
    }
    else
    {
        Debug.Log("NO ANIMATOR - releasing immediately");
        ReleaseWeapon();
    }
}


    // Called by the Animation Event (through PlayerAnimationEvents)
    // on the exact release frame of Player_WeaponThrow.
    public void ReleaseWeapon()
    {
        if (!throwInProgress || equippedShadow == null)
        {
            throwInProgress =
                false;

            return;
        }


        // Remove fake weapon from player's hand.
        if (heldWeaponVisual != null)
        {
            heldWeaponVisual.SetActive(false);
        }


        // Put the REAL gameplay weapon at the hand.
        if (throwOrigin != null)
        {
            equippedShadow.transform.position =
                throwOrigin.position;
        }


        // Launch it using the aim direction
        // saved when Attack was pressed.
        equippedShadow.Throw(
            cachedAttackAim,
            attackSpeed
        );


        equippedShadow =
            null;


        throwInProgress =
            false;
    }


    // --------------------------------------------------
    // EQUIPPED WEAPON MANAGEMENT
    // --------------------------------------------------

    public void ConsumeEquippedShadow()
    {
        if (equippedShadow == null)
            return;


        heldVisualPending = false;


        if (heldWeaponVisual != null)
        {
            heldWeaponVisual.SetActive(false);
        }


        equippedShadow.HideWorldVisual();

        equippedShadow.enabled =
            false;


        equippedShadow =
            null;


        throwInProgress =
            false;
    }


    // --------------------------------------------------
    // PLAYER STATE
    // --------------------------------------------------

    private void PlayerDeath()
    {
        ClearTarget();

        heldVisualPending = false;


        if (heldWeaponVisual != null)
        {
            heldWeaponVisual.SetActive(false);
        }


        enabled =
            false;
    }


    public void NormalAttackEnabler()
    {
        normalAttackAllowed =
            true;
    }
    public void WeaponReachedHand(ShadowRange weapon)
{
    if (weapon == null)
        return;

    if (weapon != equippedShadow)
        return;

    if (heldWeaponRenderer != null)
    {
        heldWeaponRenderer.sprite = weapon.WeaponSprite;
    }

    if (heldWeaponVisual != null)
    {
        heldWeaponVisual.SetActive(true);
    }

    weapon.HideWorldVisual();
}
}