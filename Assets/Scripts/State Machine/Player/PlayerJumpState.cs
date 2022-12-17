using Mortem.Control;
using Mortem.Core;
using Mortem.Movement;
using UnityEngine;

namespace Mortem.StateMachine.Player
{
    public class PlayerJumpState : PlayerBaseState
    {
        private PlayerInput input;
        private Mover mover;
        private ForceReceiver forceReceiver;

        private Vector3 currentMomentum;

        private bool alreadyDoubleJumped = false;

        private const float fallingThreshold = 0.2f;

        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) 
        { 
            input = stateMachine.Input;
            mover = stateMachine.Mover;
            forceReceiver = stateMachine.ForceReceiver;

            currentMomentum = stateMachine.ForceReceiver.GetMomentum();
        } 

        public override void Enter() 
        { 
            input.JumpEvent += DoubleJump;

            Jump();
        }

        public override void Tick(float deltaTime)
        {
            mover.Move(currentMomentum);

            if(forceReceiver.IsFalling(fallingThreshold))
            {
                stateMachine.SwitchState(new PlayerFallState(stateMachine));
                return;
            }
        }

        public override void Exit() 
        { 
            input.JumpEvent -= DoubleJump;
        }

        private void Jump()
        {
            forceReceiver.Jump(stateMachine.JumpForce);
            PlayAnimationSmoothly("Character_Jump");
        }

        private void DoubleJump()
        {
            if(alreadyDoubleJumped) return;

            forceReceiver.Jump(stateMachine.DoubleJumpForce);
            PlayAnimationSmoothly("Character_DoubleJump");

            alreadyDoubleJumped = true;
        }
    }
}