using Mortem.Combat;
using Mortem.Control;
using Mortem.Core;
using Mortem.Movement;
using UnityEngine;

namespace Mortem.StateMachine.Player
{
    public class PlayerLocomotionState : PlayerBaseState
    {
        private PlayerInput input;
        private Mover mover;
        private ForceReceiver forceReceiver;
        private Targeter targeter;

        private Transform mainCameraTransform;

        public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine) 
        { 
            input = stateMachine.Input;
            mover = stateMachine.Mover;
            forceReceiver = stateMachine.ForceReceiver;
            mainCameraTransform = stateMachine.MainCameraTransform;
            targeter = stateMachine.Targeter;
        }

        public override void Enter() 
        { 
            input.JumpEvent += HandleJump;
            input.TargetEvent += HandleTarget;

            PlayAnimationSmoothly("Locomotion");
        }

        public override void Tick(float deltaTime)
        {
            Jog(deltaTime);

            if(input.IsRunning)
            {
                Run(deltaTime);
            }

            if(input.IsAttacking)
            {
                Attack();
            }
        }

        public override void Exit() 
        { 
            input.JumpEvent -= HandleJump;
            input.TargetEvent -= HandleTarget;
        }

        private void Jog(float deltaTime)
        {
            if(CalculateMovement() == Vector3.zero) return;

            mover.Move(CalculateMovement() * stateMachine.FreeMovementSpeed);
            mover.LookAt(CalculateMovement());

            SetAnimationBlendTree("MovementSpeed", CalculateMovement().magnitude, deltaTime);
        }

        private void Run(float deltaTime)
        {
            if(CalculateMovement() == Vector3.zero) return;

            mover.Move(CalculateMovement() * stateMachine.RunSpeed);
            mover.LookAt(CalculateMovement());

            SetAnimationBlendTree("MovementSpeed", 1.5f, deltaTime);
        }

        private void Attack()
        {
            if(!stateMachine.Weapon) return;

            stateMachine.SwitchState(new PlayerAttackState(stateMachine, 0));
        }

        private Vector3 CalculateMovement()
        {
            Vector3 forward = GetNormalizedVector(mainCameraTransform.forward);
            Vector3 right = GetNormalizedVector(mainCameraTransform.right);

            return forward * input.MovementValue.y + right * input.MovementValue.x;
        }

        private Vector3 GetNormalizedVector(Vector3 vector)
        {
            Vector3 normalizedVector = vector.normalized;
            normalizedVector.y = 0f;

            return normalizedVector;
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
