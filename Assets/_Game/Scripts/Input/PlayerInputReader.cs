using UnityEngine;

namespace Shadowed.Input
{
    public class PlayerInputReader : MonoBehaviour
    {
        private GameInputActions inputActions;

        public float MoveInput { get; private set; }
        public Vector2 AimInput { get; private set; }
        public bool JumpRequested { get; private set; }

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
            MoveInput = inputActions.Player.Move.ReadValue<float>();
            AimInput = inputActions.Player.Aim.ReadValue<Vector2>();

            if (inputActions.Player.Jump.WasPressedThisFrame())
            {
                JumpRequested = true;
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
    }
}
