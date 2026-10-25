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
foreach (AnimatorControllerParameter parameter in animator.parameters)
{
    Debug.Log(parameter.name + " - " + parameter.type);
}

        // Player starts with empty hands.
        if (heldWeaponVisual != null)
        {
            heldWeaponVisual.SetActive(false);
        }
    }


    private void OnEnable()
    {
        playerHealth.OnDeath +=
            PlayerDeath;
    }


    private void OnDisable()
    {
        playerHealth.OnDeath -=
            PlayerDeath;
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
    }


    // --------------------------------------------------
    // TARGETING
    // --------------------------------------------------

    private void FindBestTarget()
    {
        bestDistance =
            Mathf.Infinity;


        if (targetShadow != null)
        {
            targetShadow.SetTargeted(false);
        }


        bestScore = -1f;
        targetShadow = null;


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


        if (targetShadow != null)
        {
            targetShadow.SetTargeted(true);
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
        if (!inputReader.ConsumeGrabRequest())
            return;


        if (targetShadow != null)
        {
            
            Debug.Log("Animator: " + animator.name);
Debug.Log("Controller: " + animator.runtimeAnimatorController.name);

animator.SetTrigger(GrabHash);

Debug.Log("Grab trigger sent");
            EquipShadow(
                targetShadow
            );
        }
    }


    private void EquipShadow(
        ShadowRange shadow)
    {
        if (equippedShadow != null)
        {
            Debug.Log(
                "A weapon is already equipped."
            );

            return;
        }


        equippedShadow =
            shadow;


        equippedShadow.Take();


        // Show the visual weapon in player's hand.
        if (heldWeaponVisual != null)
        {
            heldWeaponVisual.SetActive(true);
        }


        OnShadowEquipped?.Invoke(
            equippedShadow
        );


        Debug.Log(
            "Equipped Shadow: " +
            equippedShadow
        );
    }


    // --------------------------------------------------
    // ATTACK
    // --------------------------------------------------

    private void AttackHandle()
    {
        if (!normalAttackAllowed)
            return;


        if (throwInProgress)
            return;


        if (!inputReader.ConsumeAttackRequest())
            return;


        if (equippedShadow == null)
            return;

       
        // Remember the player's aim at the moment
        // the attack button is pressed.
        cachedAttackAim =
            new Vector3(
                aimDirection.x,
                aimDirection.y,
                0f
            );


        throwInProgress =
            true;


        // Start Player_WeaponThrow animation.
        animator.SetTrigger(
            ThrowHash
        );
         Debug.Log("ITs Working");
    }


    // Called by the Animation Event
    // on the exact release frame.
    public void ReleaseWeapon()
    {
        Debug.Log("=== RELEASE WEAPON CALLED ===");
        if (equippedShadow == null)
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
        equippedShadow.transform.position =
            throwOrigin.position;

         Debug.Log("Throw Aim: " + cachedAttackAim);
         Debug.Log("Throw Origin: " + throwOrigin.position);
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


        if (heldWeaponVisual != null)
        {
            heldWeaponVisual.SetActive(false);
        }


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
}