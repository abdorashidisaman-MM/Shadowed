using Shadowed.Input;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform position;
    [SerializeField] GameObject riggedCharacter;

    private ShadowSurfaceMotor motor;
    private PlayerInputReader playerInput;
    private PlayerShadowInteraction shadowInteraction;
    private ShadowSurface shadowSurface;

    private bool wasAirborne;

    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    private static readonly int AirborneHash =
        Animator.StringToHash("Airborne");

    private static readonly int VerticalVelocityHash =
        Animator.StringToHash("VerticalVelocity");

    private static readonly int LandHash =
        Animator.StringToHash("Land");
        [SerializeField] private Transform characterFacing;


    private void Awake()
    {
        motor =
            GetComponent<ShadowSurfaceMotor>();

        playerInput =
            GetComponent<PlayerInputReader>();
        shadowInteraction =
            GetComponent<PlayerShadowInteraction>();
        shadowSurface =
            GetComponent<ShadowSurface>();
    }
    

    private void LateUpdate()
    {
        bool airborne =
            motor.IsAirborne;

        float speed =
            Mathf.Abs(playerInput.MoveInput.x);


        // LANDING HAPPENED THIS FRAME
        if (wasAirborne && !airborne)
        {
            animator.SetTrigger(LandHash);
        }


        animator.SetFloat(
            SpeedHash,
            speed
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
            float horizontalInput = playerInput.MoveInput.x;

if (horizontalInput > 0.01f)
{
    characterFacing.localScale =
        new Vector3(1f, 1f, 1f);
}
else if (horizontalInput < -0.01f)
{
    characterFacing.localScale =
        new Vector3(-1f, 1f, 1f);
}
    }
   public void ChangeRootPosition()
    {
        gameObject.transform.position=position.position;
        riggedCharacter.transform.localPosition = new Vector3(0.86f,-1.97f,0);
    }
    public void RetrieveControl()
    {
        motor.enabled=true;
        playerInput.enabled=true;
        shadowInteraction.enabled=true;
        
        
    }
}