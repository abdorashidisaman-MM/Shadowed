using UnityEngine;
using Shadowed.Input;

namespace Shadowed.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float jumpSpeed =7f;
        [SerializeField] private float acceleration = 35f;
        [SerializeField] private float deceleration = 45f; 
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        private Rigidbody rb;
        private GameInputActions inputActions;
        private float moveInput;
        private Vector2 aimInput = new Vector2();
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
            ReadInput();
            
                
            
            
        }
        private void ReadInput()
        {
            moveInput = inputActions.Player.Move.ReadValue<float>();
            aimInput = inputActions.Player.Aim.ReadValue<Vector2>();

            if (inputActions.Player.Jump.WasPressedThisFrame())
            {
                jumpRequested = true;
            }

        }

        private void FixedUpdate()
        {
            CheckGrounded();
            HandleHorizontalMovement();
            HandleJump();
        }
        private void HandleHorizontalMovement()
        {
            Vector3 velocity = rb.linearVelocity;

            float targetSpeed = moveInput * moveSpeed;

            float changeRate;

            if (Mathf.Abs(moveInput) > 0.01f)
            {
                changeRate = acceleration;
            }
            else
            {
                changeRate = deceleration;
            }

            velocity.x = Mathf.MoveTowards(
                velocity.x,
                targetSpeed,
                changeRate * Time.fixedDeltaTime
            );

            rb.linearVelocity = velocity;
        }
        private void CheckGrounded()
        {
            isGrounded = Physics.CheckSphere(
                groundCheck.position,
                groundCheckRadius,
                groundLayer,
                QueryTriggerInteraction.Ignore
            );
        }
        private void HandleJump()
        {
        if (jumpRequested && isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            Vector3 velocity = rb.linearVelocity;

            velocity.y = jumpSpeed;

            rb.linearVelocity = velocity;
        }
        jumpRequested = false;
        }
        private void OnDrawGizmosSelected()
        {
            if (groundCheck == null)
            {
                return;
            }

            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundCheckRadius
            );
        }
    }
}