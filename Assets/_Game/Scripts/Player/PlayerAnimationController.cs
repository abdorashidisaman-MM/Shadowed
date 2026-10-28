using Shadowed.Input;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animator")]
    [Tooltip("The RiggedCharacter Animator that runs the Player controller.")]
    [SerializeField]
    private Animator animator;

    [Header("Visual Hierarchy")]
    [Tooltip("Left/right flip only. Never flip the gameplay root.")]
    [SerializeField] private Transform characterFacing;

    [Tooltip("Visual root that Timelines are allowed to move (RealVisual). " +
             "When control is returned, any offset left on it is transferred to the gameplay root " +
             "so the character does not visually jump.")]
    [SerializeField] private Transform timelineVisualRoot;

    [Header("Legacy (AllyReleasePoint signal)")]
    [SerializeField]
    private Transform position;
    [SerializeField] GameObject riggedCharacter;

    [Header("Tuning")]
    [Tooltip("Minimum horizontal input before the character flips facing.")]
    [SerializeField] private float facingDeadZone = 0.01f;

    private ShadowSurfaceMotor motor;
    private PlayerInputReader playerInput;
    private PlayerShadowInteraction shadowInteraction;

    private bool wasAirborne;
    private bool initialized;
    private Vector3 lastVisualRootOffset;

    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    private static readonly int AirborneHash =
        Animator.StringToHash("Airborne");

    private static readonly int VerticalVelocityHash =
        Animator.StringToHash("VerticalVelocity");

    private static readonly int LandHash =
        Animator.StringToHash("Land");


    private void Awake()
    {
        motor =
            GetComponent<ShadowSurfaceMotor>();

        playerInput =
            GetComponent<PlayerInputReader>();

        shadowInteraction =
            GetComponent<PlayerShadowInteraction>();
    }


    private void LateUpdate()
    {
        if (animator == null || motor == null || playerInput == null)
            return;

        bool airborne =
            motor.IsAirborne;

        // While gameplay movement is disabled (cutscenes, ally launch,
        // hovering after the launch) the character is not moving, so
        // the Animator must not be told to run or flip.
        bool movementActive =
            motor.enabled && playerInput.enabled;

        float horizontalInput =
            movementActive ? playerInput.MoveInput.x : 0f;


        if (!initialized)
        {
            // Avoid a fake "landing" (or take-off) on the first frame.
            wasAirborne = airborne;
            initialized = true;
        }


        if (airborne && !wasAirborne)
        {
            // Took off: discard any Land trigger that was never consumed
            // so it cannot fire on a later, unrelated fall.
            animator.ResetTrigger(LandHash);
        }
        else if (wasAirborne && !airborne)
        {
            // LANDING HAPPENED THIS FRAME
            animator.SetTrigger(LandHash);
        }


        animator.SetFloat(
            SpeedHash,
            Mathf.Abs(horizontalInput)
        );

        animator.SetBool(
            AirborneHash,
            airborne
        );

        animator.SetFloat(
            VerticalVelocityHash,
            motor.VerticalVelocity
        );


        wasAirborne =
            airborne;


        if (characterFacing != null)
        {
            if (horizontalInput > facingDeadZone)
            {
                characterFacing.localScale =
                    new Vector3(1f, 1f, 1f);
            }
            else if (horizontalInput < -facingDeadZone)
            {
                characterFacing.localScale =
                    new Vector3(-1f, 1f, 1f);
            }
        }


        // Remember where Timeline left the visual root this frame.
        if (timelineVisualRoot != null)
        {
            lastVisualRootOffset =
                timelineVisualRoot.localPosition;
        }
    }


    // Called by the (currently unused) AllyReleasePoint signal.
    // The permanent character-to-collider offset now lives on VisualOffset,
    // so this must NOT write a hard-coded offset onto RiggedCharacter anymore
    // (doing so doubled the offset).
    public void ChangeRootPosition()
    {
        if (position != null && position != transform)
        {
            transform.position = position.position;
        }
    }


    // Signal: RetrieveControl (Hair execution, end of the player's part).
    public void RetrieveControl()
    {
        TransferVisualOffsetToRoot();

        motor.enabled = true;
        playerInput.enabled = true;
        shadowInteraction.enabled = true;

        // Timelines may have left the root slightly off the surface (depth).
        motor.ResnapToCurrentSurface();
    }


    // A Timeline animates the visual root (RealVisual) while the gameplay
    // root stays still. When the player gets control back, move the gameplay
    // root to where the visual ended up and zero the visual root, so the
    // character neither jumps visually nor stays separated from its collider.
    private void TransferVisualOffsetToRoot()
    {
        if (timelineVisualRoot == null)
            return;

        Vector3 offset =
            timelineVisualRoot.localPosition != Vector3.zero
                ? timelineVisualRoot.localPosition
                : lastVisualRootOffset;

        if (offset.sqrMagnitude < 0.000001f)
            return;

        Vector3 worldOffset =
            timelineVisualRoot.parent != null
                ? timelineVisualRoot.parent.TransformVector(offset)
                : offset;

        // Only carry over the in-plane part; depth is owned by the motor.
        worldOffset.z = 0f;

        transform.position += worldOffset;

        timelineVisualRoot.localPosition = Vector3.zero;
        lastVisualRootOffset = Vector3.zero;
    }
}