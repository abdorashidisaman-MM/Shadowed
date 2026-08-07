using UnityEngine;
using Shadowed.Input;

namespace Shadowed.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float jumpSpeed =7f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        private Rigidbody rb;
        private GameInputActions inputActions;
        private float moveInput;
        private bool isGrounded;
        private bool jumpRequested; 

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputActions = new GameInputActions();
        }

        private void OnEnable()
        {
            inputActions.Player.Enable();
        }

        private void OnDisable()
        {
            inputActions.Player.Disable();
        }

        private void Update()
        {
            moveInput = inputActions.Player.Move.ReadValue<float>();
            if(inputActions.Player.Jump.WasPressedThisFrame())
            {
                jumpRequested = true;
            }
        }

        private void FixedUpdate()
        {
            isGrounded=Physics.CheckSphere(
                groundCheck.position,
                groundCheckRadius,
                groundLayer,
                QueryTriggerInteraction.Ignore
            );
            Vector3 velocity = rb.linearVelocity;

            velocity.x = moveInput * moveSpeed;
            if (jumpRequested && isGrounded && velocity.y <= 0.1f)
            {
                velocity.y = jumpSpeed;
            }


            rb.linearVelocity = velocity;
            jumpRequested = false;

        }
    }
}