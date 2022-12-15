using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mortem.Control
{
    public class PlayerInput : MonoBehaviour, Controls.IPlayerActions
    {
        public Vector2 MovementValue { get; private set; }

        public bool IsRunning { get; private set; }
        public bool IsAttacking { get; private set; }

        public event Action JumpEvent;
        public event Action TargetEvent;

        private Controls controls;
        
        private void Start()
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
            controls.Player.Enable();
        }

        private void OnDestroy()
        {
            controls.Player.Disable();
        }

        public void ToggleCursor(bool state)
        {
            Cursor.visible = state;

            if(!state) 
            {
                Cursor.lockState = CursorLockMode.Locked;
                return;
            }

            Cursor.lockState = CursorLockMode.None;
        }

        public void OnLocomotion(InputAction.CallbackContext context)
        {
            MovementValue = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(!context.performed) return;

            JumpEvent?.Invoke();
        }
        
        public void OnRun(InputAction.CallbackContext context)
        {
            IsRunning = context.performed;
        }

        public void OnTarget(InputAction.CallbackContext context)
        {
            if(!context.performed) return;

            TargetEvent?.Invoke();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            IsAttacking = context.performed;
        }

        public void OnLook(InputAction.CallbackContext context) { }
    }
}
