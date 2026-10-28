using UnityEngine;

namespace Shadowed.Input
{
    public class PlayerInputReader : MonoBehaviour
    {
        [Header("Buffering")]
        [Tooltip("How long (seconds) a button press stays available to be consumed. " +
                 "Prevents presses made during cutscenes / disabled states from firing much later.")]
        [SerializeField] private float requestBufferTime = 0.2f;

        private GameInputActions inputActions;

        // Time at which each request was made. Negative = no pending request.
        private float jumpRequestTime = -1f;
        private float grabRequestTime = -1f;
        private float attackRequestTime = -1f;
        private float specialRequestTime = -1f;

        public Vector2 MoveInput { get; private set; }
        public Vector2 AimInput { get; private set; }
        public bool JumpRequested => IsPending(jumpRequestTime);
        public bool GrabRequest => IsPending(grabRequestTime);
        public bool AttackRequest => IsPending(attackRequestTime);
        public bool SpecialRequest => IsPending(specialRequestTime);

        private void Awake()
        {
            inputActions = new GameInputActions();
        }

        private void OnEnable()
        {
            inputActions.Player.Enable();
        }

        private void OnDisable()
        {
            inputActions.Player.Disable();
            MoveInput = Vector2.zero;
            AimInput = Vector2.zero;
            ClearRequests();
        }

        private void OnDestroy()
        {
            inputActions?.Dispose();
        }

        private void Update()
        {
            MoveInput = inputActions.Player.Move.ReadValue<Vector2>();
            AimInput = inputActions.Player.Aim.ReadValue<Vector2>();

            float now = Time.time;

            if (inputActions.Player.Grab.WasPressedThisFrame())
                grabRequestTime = now;

            if (inputActions.Player.Special.WasPressedThisFrame())
                specialRequestTime = now;

            if (inputActions.Player.Jump.WasPressedThisFrame())
                jumpRequestTime = now;
if (inputActions.Player.Attack.WasPressedThisFrame())
{
    Debug.Log("INPUT ATTACK DETECTED");
    attackRequestTime = now;
}
        }

        public void ClearRequests()
        {
            jumpRequestTime = -1f;
            grabRequestTime = -1f;
            attackRequestTime = -1f;
            specialRequestTime = -1f;
        }

        private bool IsPending(float requestTime)
        {
            return requestTime >= 0f && Time.time - requestTime <= requestBufferTime;
        }

        private bool Consume(ref float requestTime)
        {
            bool pending = IsPending(requestTime);
            requestTime = -1f;
            return pending;
        }

        public bool ConsumeJumpRequest() => Consume(ref jumpRequestTime);

        public bool ConsumeGrabRequest() => Consume(ref grabRequestTime);

        public bool ConsumeAttackRequest() => Consume(ref attackRequestTime);

        public bool ConsumeSpecialRequest() => Consume(ref specialRequestTime);
    }
}
