using UnityEngine;

namespace Shadowed.Input
{
    public class PlayerInputReader : MonoBehaviour
    {
        private GameInputActions inputActions;

        public Vector2 MoveInput { get; private set; }
        public Vector2 AimInput { get; private set; }
        public bool JumpRequested { get; private set; }
        public bool GrabRequest {get; private set; }
        public bool AttackRequest {get; private set; }
        public bool SpecialRequest {get; private set; }

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
        }

        private void Update()
        {
            MoveInput = inputActions.Player.Move.ReadValue<Vector2>();
            AimInput = inputActions.Player.Aim.ReadValue<Vector2>();
            if(inputActions.Player.Grab.WasPressedThisFrame())
            {
                GrabRequest = true;
            }
             if(inputActions.Player.Special.WasPressedThisFrame())
            {
                SpecialRequest = true;
            }

            if (inputActions.Player.Jump.WasPressedThisFrame())
            {
                JumpRequested = true;
            }
            if (inputActions.Player.Attack.WasPressedThisFrame())
            {
                AttackRequest = true;
            }
        }

        public bool ConsumeJumpRequest()
        {
            if (JumpRequested)
            {
                JumpRequested = false;
                return true;
            }

            return false;
        }
        
        public bool ConsumeGrabRequest()
        {
            if (GrabRequest)
            {
                GrabRequest = false;
                return true;
            }

            return false;
        }
        public bool ConsumeAttackRequest()
        {
            if (AttackRequest)
            {
                AttackRequest = false;
                return true;
            }

            return false;
        }
         public bool ConsumeSpecialRequest()
        {
            if (SpecialRequest)
            {
                SpecialRequest = false;
                return true;
            }

            return false;
        }
    }
}
