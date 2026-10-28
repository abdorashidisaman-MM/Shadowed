using Shadowed.Input;
using UnityEngine;

public class ShadowSurfaceMotor : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;

    [Header("Jump And Gravity")]
    [SerializeField] private float jumpSpeed = 7f;
    [SerializeField] private float gravity = -20f;

    [Header("Landing")]
    [SerializeField] private float feetOffset = 2f;

    [Header("Surface")]
    [SerializeField] private float surfaceOffset = 0.5f;
    [SerializeField] private float initialSurfaceCheckDistance = 2f;
    [SerializeField] private float cornerInset = 0.1f;
    [Tooltip("When the player is first attached to a surface, if it is within this distance " +
             "above the surface floor it starts grounded (Idle) instead of falling.")]
    [SerializeField] private float initialGroundSnapDistance = 0.5f;
    [SerializeField] private LayerMask surfaceMask;


    private PlayerInputReader playerInput;

    private ShadowSurface currentSurface;

    private float verticalVelocity;

    private bool isSupported;


    public bool IsSupported =>
        isSupported;


    public bool IsAirborne =>
        currentSurface != null &&
        !isSupported;


    public float VerticalVelocity =>
        verticalVelocity;


    private void Awake()
    {
        playerInput =
            GetComponent<PlayerInputReader>();
    }


    private void Update()
    {
        // -----------------------------
        // FIND INITIAL SURFACE
        // -----------------------------

        if (currentSurface == null)
        {
            if (!TryFindInitialSurface())
            {
                return;
            }
        }


        // -----------------------------
        // INPUT
        // -----------------------------

        float horizontalInput =
            playerInput.MoveInput.x;


        // -----------------------------
        // JUMP
        // -----------------------------

        if (playerInput.ConsumeJumpRequest())
        {
            if (isSupported)
            {
                verticalVelocity =
                    jumpSpeed;

                isSupported =
                    false;
            }
        }


        // -----------------------------
        // GRAVITY
        // -----------------------------

        if (!isSupported)
        {
            verticalVelocity +=
                gravity * Time.deltaTime;
        }


        // -----------------------------
        // MOVEMENT
        // -----------------------------

        HandleVerticalMovement();

        HandleHorizontalMovement(
            horizontalInput
        );
    }


    private bool TryFindInitialSurface()
    {
        if (!Physics.Raycast(
            transform.position,
            transform.forward,
            out RaycastHit hit,
            initialSurfaceCheckDistance,
            surfaceMask,
            QueryTriggerInteraction.Ignore))
        {
            return false;
        }


        ShadowSurface surface =
            hit.collider.GetComponent<ShadowSurface>();


        if (surface == null)
        {
            return false;
        }


        currentSurface =
            surface;


        if (currentSurface.TrySnapPosition(
            transform.position,
            surfaceOffset,
            out Vector3 snappedPosition))
        {
            transform.position =
                snappedPosition;
        }
        else
        {
            transform.position =
                hit.point +
                currentSurface.Normal *
                surfaceOffset;
        }


        verticalVelocity = 0f;

        // If we spawned standing on (or just above) the floor of the
        // surface, start grounded. Previously this was always false,
        // which made the player fall a few centimetres on the first
        // frame and play Jump -> Fall -> Land with no input.
        isSupported =
            currentSurface.TrySnapToBottom(
                transform.position - Vector3.up * initialGroundSnapDistance,
                surfaceOffset,
                feetOffset,
                out Vector3 groundedPosition);

        if (isSupported)
        {
            transform.position =
                groundedPosition;
        }


        return true;
    }


    private void HandleVerticalMovement()
    {
        if (Mathf.Abs(verticalVelocity) < 0.001f)
        {
            return;
        }


        Vector3 verticalStep =
            Vector3.up *
            verticalVelocity *
            Time.deltaTime;


        Vector3 desiredPosition =
            transform.position +
            verticalStep;


        // -----------------------------
        // LANDING CHECK
        // -----------------------------

        if (verticalVelocity < 0f)
        {
            if (currentSurface.TrySnapToBottom(
                desiredPosition,
                surfaceOffset,
                feetOffset,
                out Vector3 landingPosition))
            {
                transform.position =
                    landingPosition;

                verticalVelocity = 0f;

                isSupported = true;

                return;
            }
        }


        // -----------------------------
        // STILL ON CURRENT WALL
        // -----------------------------

        if (currentSurface.TrySnapPosition(
            desiredPosition,
            surfaceOffset,
            out Vector3 snappedPosition))
        {
            transform.position =
                snappedPosition;

            isSupported =
                false;

            return;
        }


        // -----------------------------
        // BOTTOM EDGE FALLBACK
        // -----------------------------

        if (verticalVelocity < 0f)
        {
            verticalVelocity = 0f;

            isSupported = true;

            return;
        }


        // -----------------------------
        // TOP EDGE
        // -----------------------------

        verticalVelocity = 0f;
    }


    private void HandleHorizontalMovement(
        float horizontalInput)
    {
        if (Mathf.Abs(horizontalInput) < 0.01f)
        {
            return;
        }
        


        Vector3 horizontalStep =
            currentSurface.Right *
            horizontalInput *
            moveSpeed *
            Time.deltaTime;


        Vector3 desiredPosition =
            transform.position +
            horizontalStep;


        // -----------------------------
        // STILL ON CURRENT WALL
        // -----------------------------

        if (currentSurface.TrySnapPosition(
            desiredPosition,
            surfaceOffset,
            out Vector3 snappedPosition))
        {
            transform.position =
                snappedPosition;

            return;
        }


        // -----------------------------
        // REACHED AN EDGE
        // -----------------------------

        ShadowSurface nextSurface =
            currentSurface.GetConnection(
                horizontalInput
            );


        if (nextSurface == null)
        {
            return;
        }


        bool enteringFromLeft =
            horizontalInput > 0f;


        Vector3 transitionPosition =
            nextSurface.GetEntryPosition(
                transform.position,
                enteringFromLeft,
                surfaceOffset,
                cornerInset
            );


        currentSurface =
            nextSurface;


        transform.position =
            transitionPosition;
    }


    // Re-applies the current surface snap (depth offset etc.) after an
    // external system (e.g. a Timeline) moved the Player root.
    public void ResnapToCurrentSurface()
    {
        if (currentSurface != null)
        {
            AttachToSurface(currentSurface);
        }
    }


    public void AttachToSurface(
        ShadowSurface surface)
    {
        if (surface == null)
        {
            return;
        }

        if (surface.TrySnapPosition(
            transform.position,
            surfaceOffset,
            out Vector3 snappedPosition))
        {
            currentSurface =
                surface;

            transform.position =
                snappedPosition;

            verticalVelocity =
                0f;

            isSupported =
                true;
        }
    }
}