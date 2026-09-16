using Shadowed.Input;
using UnityEngine;

public class ShadowSurfaceMotor : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
   
    [Header("Jump and Gravity")]
    [SerializeField] private float jumpSpeed = 7f;
    [SerializeField] private float gravity = -20f;

    private float verticalVelocity;
    private bool jumpRequested;
    private bool isSupported;

    [Header("Surface")]
    [SerializeField] private float surfaceOffset = 0.5f;
    [SerializeField] private float surfaceCheckDistance = 1f;
    [SerializeField] private LayerMask surfaceMask;

    [Header("Corner Detection")]
    [SerializeField] private float edgeProbeDistance = 0.6f;
    [SerializeField] private float cornerProbeDistance = 1f;
    [SerializeField] private float cornerInset = 0.05f;

    private PlayerInputReader playerInput;

    private Vector2 moveInput;

    private RaycastHit currentSurfaceHit;

    private Vector3 currentSurfaceNormal;

    private Vector3 surfaceUp;
    private Vector3 surfaceRight;

    private bool hasSurface;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInputReader>();
    }


    private void Update()
    {
        moveInput = playerInput.MoveInput;
        if (!hasSurface)
        {
            if (!TryInitialSurface())
            {
                return;
            }

            AttachToSurface();
        }

       if(playerInput.ConsumeJumpRequest())
        {
            jumpRequested=true;
            
        }
       if (!isSupported)
        {
        verticalVelocity += gravity * Time.deltaTime;
        }
        if(jumpRequested)
        {
            if(isSupported)
            {
                verticalVelocity=jumpSpeed;
                isSupported=false;
            }
            jumpRequested=false;
        }
        HandleVerticalMovement();

        HandleHorizontalMovement();
        // -----------------------------
        // INITIAL SURFACE
        // -----------------------------

        


        // -----------------------------
        // MOVEMENT
        // -----------------------------

        Vector3 movement =
            CalculateSurfaceMovement();

        if (movement.sqrMagnitude == 0f)
        {
            return;
        }


        Vector3 nextPosition =
            transform.position +
            movement * moveSpeed * Time.deltaTime;


        // -----------------------------
        // CURRENT FACE CONTINUES
        // -----------------------------

        if (TryFindSurfaceAtNextPosition(
            nextPosition,
            out RaycastHit nextSurfaceHit))
        {
            currentSurfaceHit =
                nextSurfaceHit;

            currentSurfaceNormal =
                nextSurfaceHit.normal;

            hasSurface = true;

            AttachToSurface();

            return;
        }


        // -----------------------------
        // OUTSIDE CORNER
        // -----------------------------

        if (TryFindCornerSurface(
            nextPosition,
            movement,
            out RaycastHit cornerHit))
        {
            TransitionToSurface(
                cornerHit
            );


            transform.position =
                cornerHit.point +
                currentSurfaceNormal *
                surfaceOffset;


            return;
        }


        // No valid surface found.
        // Player remains where it is.
    }


    private Vector3 CalculateSurfaceMovement()
    {
        Vector3 movement =
            surfaceRight * moveInput.x;


        if (movement.sqrMagnitude > 0f)
        {
            movement.Normalize();
        }


        return movement;
    }
    
    private void HandleVerticalMovement()
        {
            if (Mathf.Abs(verticalVelocity) < 0.001f)
            {
                return;
            }

            Vector3 verticalStep =
                surfaceUp *
                verticalVelocity *
                Time.deltaTime;

            Vector3 nextPosition =
                transform.position +
                verticalStep;


            if (TryFindSurfaceAtNextPosition(
                nextPosition,
                out RaycastHit nextSurfaceHit))
            {
                currentSurfaceHit =
                    nextSurfaceHit;

                currentSurfaceNormal =
                    nextSurfaceHit.normal;

                transform.position =
                    currentSurfaceHit.point +
                    currentSurfaceNormal *
                    surfaceOffset;

                isSupported = false;

                return;
            }


            // We tried to move downward,
            // but there is no more wall below us.
            if (verticalVelocity < 0f)
            {
                verticalVelocity = 0f;
                isSupported = true;
            }

            // We tried to move upward,
            // but reached the top edge of the wall.
            else
            {
                verticalVelocity = 0f;
            }
        }
        private void HandleHorizontalMovement()
{
    float horizontalInput = moveInput.x;

    if (Mathf.Abs(horizontalInput) < 0.01f)
    {
        return;
    }

    Vector3 horizontalDirection =
        surfaceRight * horizontalInput;

    Vector3 nextPosition =
        transform.position +
        horizontalDirection *
        moveSpeed *
        Time.deltaTime;

    if (TryFindSurfaceAtNextPosition(
        nextPosition,
        out RaycastHit nextSurfaceHit))
    {
        currentSurfaceHit =
            nextSurfaceHit;

        currentSurfaceNormal =
            nextSurfaceHit.normal;

        transform.position =
            currentSurfaceHit.point +
            currentSurfaceNormal *
            surfaceOffset;

        return;
        }

        if (TryFindCornerSurface(
            nextPosition,
            horizontalDirection,
            out RaycastHit cornerHit))
        {
            TransitionToSurface(cornerHit);

            transform.position =
                cornerHit.point +
                currentSurfaceNormal *
                surfaceOffset;
        }
    }

    private bool TryInitialSurface()
    {
        Vector3 direction =
            transform.forward;


        if (Physics.Raycast(
            transform.position,
            direction,
            out RaycastHit hit,
            surfaceCheckDistance,
            surfaceMask))
        {
            currentSurfaceHit =
                hit;

            currentSurfaceNormal =
                hit.normal;


            InitializeSurfaceBasis();


            hasSurface = true;


            return true;
        }


        return false;
    }


    private bool TryFindSurfaceAtNextPosition(
        Vector3 nextPosition,
        out RaycastHit hit)
    {
        Vector3 direction =
            -currentSurfaceNormal;


        if (!Physics.Raycast(
            nextPosition,
            direction,
            out hit,
            surfaceCheckDistance,
            surfaceMask))
        {
            return false;
        }


        float normalAlignment =
            Vector3.Dot(
                currentSurfaceNormal.normalized,
                hit.normal.normalized
            );


        bool sameFace =
            normalAlignment > 0.99f;


        return sameFace;
    }


    private bool TryFindCornerSurface(
        Vector3 nextPosition,
        Vector3 movement,
        out RaycastHit cornerHit)
    {
        Vector3 cornerProbeOrigin =
            nextPosition
            + movement * edgeProbeDistance
            - currentSurfaceNormal *
            (surfaceOffset + cornerInset);


        Vector3 cornerProbeDirection =
            -movement;


        if (!Physics.Raycast(
            cornerProbeOrigin,
            cornerProbeDirection,
            out cornerHit,
            cornerProbeDistance,
            surfaceMask))
        {
            return false;
        }


        Vector3 candidateNormal =
            cornerHit.normal.normalized;


        Vector3 normalizedMovement =
            movement.normalized;


        // Candidate face should be roughly
        // perpendicular to the current face.
        float surfaceAngle =
            Mathf.Abs(
                Vector3.Dot(
                    currentSurfaceNormal.normalized,
                    candidateNormal
                )
            );


        bool isDifferentFace =
            surfaceAngle < 0.1f;


        // For an outside corner,
        // the new face normal should point
        // approximately in the direction
        // we were travelling.
        float movementAlignment =
            Vector3.Dot(
                candidateNormal,
                normalizedMovement
            );


        bool isCorrectCorner =
            movementAlignment > 0.9f;


        return
            isDifferentFace &&
            isCorrectCorner;
    }


    private void TransitionToSurface(RaycastHit newSurfaceHit)
{
    Vector3 newNormal =
        newSurfaceHit.normal.normalized;


    currentSurfaceHit =
        newSurfaceHit;

    currentSurfaceNormal =
        newNormal;

    hasSurface = true;


    // ---------------------------------
    // VERTICAL WALL
    // ---------------------------------

    float verticalAlignment =
        Mathf.Abs(
            Vector3.Dot(
                newNormal,
                Vector3.up
            )
        );


    bool isVerticalWall =
        verticalAlignment < 0.1f;


    if (isVerticalWall)
    {
        surfaceUp =
            Vector3.up;


        surfaceRight =
            Vector3.Cross(
                newNormal,
                surfaceUp
            ).normalized;


        return;
    }


    // Floor / ceiling behavior
    // will be designed later.
}


    private void InitializeSurfaceBasis()
    {
        Vector3 referenceUp =
            Vector3.up;


        // Vector3.up cannot be used if
        // the surface itself faces up/down.
        if (Mathf.Abs(
            Vector3.Dot(
                currentSurfaceNormal,
                Vector3.up
            )) > 0.99f)
        {
            referenceUp =
                Vector3.forward;
        }


        surfaceRight =
            Vector3.Cross(
                currentSurfaceNormal,
                referenceUp
            ).normalized;


        surfaceUp =
            Vector3.Cross(
                surfaceRight,
                currentSurfaceNormal
            ).normalized;
    }


    private void AttachToSurface()
    {
        if (!hasSurface)
        {
            return;
        }


        transform.position =
            currentSurfaceHit.point +
            currentSurfaceNormal *
            surfaceOffset;
    }
}