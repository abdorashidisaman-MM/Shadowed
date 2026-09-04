using UnityEngine;
using Shadowed.Input;

namespace Shadowed.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInputReader))]
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
        private float moveInput;
        private bool isGrounded;
        private bool jumpRequested; 
        private PlayerInputReader inputReader;
        private PlayerHealth playerHealth;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputReader=GetComponent<PlayerInputReader>();
            playerHealth=GetComponent<PlayerHealth>();
        }
        void OnEnable()
        {
            playerHealth.OnDeath+=PlayerDeath;
        }
        void OnDisable()
        {
            playerHealth.OnDeath-=PlayerDeath;
        }

        private void Update()
        {
            ReadInput();
            
               
            
            
        }
        private void ReadInput()
        {
            moveInput = inputReader.MoveInput;
           

            if (inputReader.ConsumeJumpRequest())
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
       private void PlayerDeath()
        {
            moveInput = 0f;
            jumpRequested = false;

            Vector3 velocity = rb.linearVelocity;
            velocity.x = 0f;
            rb.linearVelocity = velocity;

            enabled = false;
        }
    }
    
}