using UnityEngine;

namespace Mortem.StateMachine.Player
{
    public class PlayerJumpState : PlayerBaseState
    {
        private const float fallingThreshold = 0.2f;

        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { } 

        public override void Enter() 
        { 
            input.JumpEvent += HandleDoubleJump;

            JumpBehaviour("Character_Jump", stateMachine.JumpForce);
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
            input.JumpEvent -= HandleDoubleJump;
        }

        private void HandleDoubleJump()
        {
            stateMachine.SwitchState(new PlayerDoubleJumpState(stateMachine));
        }
    }
}