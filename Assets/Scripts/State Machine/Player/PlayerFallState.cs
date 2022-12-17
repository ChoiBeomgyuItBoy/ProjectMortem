using Mortem.Core;
using UnityEngine;

namespace Mortem.StateMachine.Player
{
    public class PlayerFallState : PlayerBaseState
    {
        private Vector3 currentMomentum;

        public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine) 
        { 
            currentMomentum = stateMachine.ForceReceiver.GetMomentum();
        }

        public override void Enter()
        {
            PlayAnimationSmoothly("Character_Fall");
        }

        public override void Tick(float deltaTime)
        {
            stateMachine.Mover.Move(currentMomentum);

            if(stateMachine.ForceReceiver.IsGrounded())
            {
                stateMachine.SwitchState(new PlayerLocomotionState(stateMachine));
            }
        }

        public override void Exit() { }
    }
}
