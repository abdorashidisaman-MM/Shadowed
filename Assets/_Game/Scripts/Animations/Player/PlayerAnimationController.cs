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

    private bool wasAirborne;

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
    }
   public void ChangeRootPosition()
    {
        gameObject.transform.position=position.position;
        riggedCharacter.transform.localPosition = new Vector3(0.86f,-1.97f,0);
    }
}