using UnityEngine;

namespace Mortem.StateMachine.Player
{
    public class PlayerLocomotionState : PlayerBaseState
    {
        private static bool alreadyEquipedWeapon;

        public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter() 
        { 
            input.JumpEvent += HandleJump;
            input.TargetEvent += HandleTarget;

            PlayAnimationSmoothly("Locomotion");

            if(!weapon) return;

            fighter.UnequipWeapon();
        }

        public override void Tick(float deltaTime)
        {
            LocomotionBehaviour(stateMachine.FreeMovementSpeed, CalculateMovement().magnitude, deltaTime);

            if(input.IsRunning)
            {
                if(CalculateMovement() == Vector3.zero) return;
                LocomotionBehaviour(stateMachine.RunningSpeed, 1.5f, deltaTime);
            }

            if(input.IsAttacking)
            {
                if(!weapon) return;
                stateMachine.SwitchState(new PlayerAttackState(stateMachine, 0));
                fighter.EquipWeapon();
            }
        }

        public override void Exit() 
        { 
            input.JumpEvent -= HandleJump;
            input.TargetEvent -= HandleTarget;
        }

        private void LocomotionBehaviour(float movementSpeed, float blendTreeValue, float deltaTime)
        {
            mover.Move(CalculateMovement() * movementSpeed);
            mover.LookAt(CalculateMovement());

            SetAnimationBlendTree("MovementSpeed", blendTreeValue, deltaTime);
        }

        private Vector3 CalculateMovement()
        {
            Vector3 forward = GetNormalizedVector(GetMainCameraTransfrom().forward);
            Vector3 right = GetNormalizedVector(GetMainCameraTransfrom().right);

            return forward * input.MovementValue.y + right * input.MovementValue.x;
        }

        private Vector3 GetNormalizedVector(Vector3 vector)
        {
            Vector3 normalizedVector = vector.normalized;
            normalizedVector.y = 0f;

            return normalizedVector;
        }

        private Transform GetMainCameraTransfrom()
        {
            return Camera.main.transform;
        }

        // Event Listeners
        private void HandleJump()
        {
            stateMachine.SwitchState(new PlayerJumpState(stateMachine));
        }

        private void HandleTarget()
        {
            if(!targeter.SelectCurrentTarget()) return;

            stateMachine.SwitchState(new PlayerTargetState(stateMachine));
        }
    }
}
