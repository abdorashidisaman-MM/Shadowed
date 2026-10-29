using UnityEngine;

public class ShadowTraversal : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private GameObject allyShadow;
    [SerializeField] private GameObject allyReleasePoint;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 20f;

    // Speed while the player is floating near the weapon.
    [SerializeField] private float slowMoveSpeed = 2f;

    // When this close to the release point,
    // movement changes from normal speed to slow speed.
    [SerializeField] private float slowDownDistance = 3f;

    [SerializeField] private float minimumdistance = 0.1f;


    [Header("References")]
    [SerializeField] private ShadowSurfaceMotor shadowSurfaceMotor;

    private PlayerShadowInteraction playerInteraction;

    private GameObject currentTarget;


    [Header("Rotation")]
    [Tooltip(
        "Degrees/second used to return the Player root " +
        "to its original rotation after the launch."
    )]
    [SerializeField] private float rotationReturnSpeed = 180f;


    private bool firstDistanceRiched = true;
    private bool secondDistanceRiched = true;

    private Quaternion restRotation = Quaternion.identity;
    private bool returningRotation;


    private void Awake()
    {
        playerInteraction =
            GetComponent<PlayerShadowInteraction>();
    }


    private void OnEnable()
    {
        if (playerInteraction != null)
        {
            playerInteraction.OnShadowEquipped += WeaponGrabbed;
        }
    }


    private void OnDisable()
    {
        if (playerInteraction != null)
        {
            playerInteraction.OnShadowEquipped -= WeaponGrabbed;
        }
    }


    private void Update()
    {
        // ---------------------------------------------
        // RETURN PLAYER ROTATION AFTER TRAVERSAL
        // ---------------------------------------------

        if (returningRotation)
        {
            transform.rotation =
                Quaternion.RotateTowards(
                    transform.rotation,
                    restRotation,
                    rotationReturnSpeed * Time.deltaTime
                );

            if (
                Quaternion.Angle(
                    transform.rotation,
                    restRotation
                ) < 0.05f
            )
            {
                transform.rotation = restRotation;
                returningRotation = false;
            }
        }


        // ---------------------------------------------
        // PHASE 1
        // PLAYER MOVES QUICKLY TOWARD ALLY SHADOW
        // ---------------------------------------------

        if (!firstDistanceRiched && !secondDistanceRiched)
        {
            currentTarget = allyShadow;

            float distance =
                Vector3.Distance(
                    transform.position,
                    allyShadow.transform.position
                );

            if (distance >= minimumdistance)
            {
                Travel(
                    currentTarget,
                    moveSpeed
                );
            }
            else
            {
                firstDistanceRiched = true;

                currentTarget =
                    allyReleasePoint;
            }
        }


        // ---------------------------------------------
        // PHASE 2
        // PLAYER FLIES TOWARD RELEASE POINT
        //
        // WHEN CLOSE:
        //     SLOW DOWN
        //
        // DO NOT FINISH TRAVERSAL HERE.
        // WE WAIT UNTIL THE WEAPON IS GRABBED.
        // ---------------------------------------------

        if (firstDistanceRiched && !secondDistanceRiched)
        {
            currentTarget =
                allyReleasePoint;


            float distance =
                Vector3.Distance(
                    transform.position,
                    allyReleasePoint.transform.position
                );


            // Normal speed by default.
            float currentSpeed =
                moveSpeed;


            // When approaching the weapon/release area,
            // slow the player's aerial movement.
            if (distance <= slowDownDistance)
            {
                currentSpeed =
                    slowMoveSpeed;
            }


            // Keep moving until we reach the point.
            //
            // If the player reaches it before grabbing
            // the weapon, they simply remain hovering there.
            if (distance >= minimumdistance)
            {
                Travel(
                    currentTarget,
                    currentSpeed
                );
            }
        }
    }


    // --------------------------------------------------
    // MOVEMENT
    // --------------------------------------------------

    public void Travel(
        GameObject target,
        float speed
    )
    {
        if (target == null)
            return;


        transform.position =
            Vector3.MoveTowards(
                transform.position,
                target.transform.position,
                speed * Time.deltaTime
            );


        transform.right =
            Vector3.RotateTowards(
                transform.right,
                target.transform.position -
                transform.position,
                1f * Time.deltaTime,
                0f
            );
    }


    // --------------------------------------------------
    // CALLED WHEN ALLY LAUNCH STARTS
    // --------------------------------------------------

    public void TraversalQue()
    {
        firstDistanceRiched = false;
        secondDistanceRiched = false;

        currentTarget =
            allyShadow;

        returningRotation = false;


        // Player movement is controlled by this script
        // during the launch.
        if (shadowSurfaceMotor != null)
        {
            shadowSurfaceMotor.enabled = false;
        }
    }


    // --------------------------------------------------
    // WEAPON GRABBED
    // --------------------------------------------------

    private void WeaponGrabbed(
        ShadowRange weapon
    )
    {
        // We only care about a weapon grab while
        // we're in the second part of the ally launch.
        if (!firstDistanceRiched)
            return;

        if (secondDistanceRiched)
            return;


        secondDistanceRiched = true;

        currentTarget = null;


        // Stop traversal movement and begin returning
        // the Player root to its normal orientation.
        returningRotation = true;

        Debug.Log(
            "Weapon grabbed - ally traversal finished."
        );
    }
}