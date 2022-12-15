using UnityEngine;

namespace Mortem.StateMachine.Player
{
    public class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            PlayAnimationSmoothly("Character_Fall");
        }

        public override void Tick(float deltaTime)
        {
            mover.Move(forceReceiver.GetMomentum());

            if(forceReceiver.IsGrounded())
            {
                stateMachine.SwitchState(new PlayerLocomotionState(stateMachine));
            }
        }

        public override void Exit() { }
    }
}
