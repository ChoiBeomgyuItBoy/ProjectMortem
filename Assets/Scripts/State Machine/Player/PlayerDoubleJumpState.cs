using UnityEngine;

namespace Mortem.StateMachine.Player
{
    public class PlayerDoubleJumpState : PlayerBaseState
    {
        private const float fallingThreshold = 0.2f;

        public PlayerDoubleJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }
        
        public override void Enter()
        {
            JumpBehaviour("Character_DoubleJump", stateMachine.DoubleJumpForce);
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

        public override void Exit() { }
    }
}