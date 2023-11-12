using UnityEngine;
using UnityEngine.InputSystem;

namespace EggWars2D.Inputs
{
    public class InputReader
    {
        readonly GameInputActions _input;

        public Vector2 ScreenTouchPosition { get; private set; }
        public bool IsTouchDown => _input.Player.Touch.WasPressedThisFrame();
        public bool IsTouch => _input.Player.Touch.ReadValue<float>() > 0.1f; 

        public InputReader()
        {
            _input = new GameInputActions();

            _input.Player.TouchPosition.performed += HandleOnTouchPosition;
            _input.Player.TouchPosition.canceled += HandleOnTouchPosition;
            
            _input.Enable();
        }
        
        ~InputReader()
        {
            _input.Player.TouchPosition.performed -= HandleOnTouchPosition;
            _input.Player.TouchPosition.canceled -= HandleOnTouchPosition;
            
            _input.Disable();
        }

        void HandleOnTouchPosition(InputAction.CallbackContext context)
        {
            ScreenTouchPosition = context.ReadValue<Vector2>();
        }
    }
}