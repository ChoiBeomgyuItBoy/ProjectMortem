using Mortem.Core;
using Mortem.Movement;
using UnityEngine;

namespace Mortem.StateMachine.Player
{
    public class PlayerImpactState : PlayerBaseState
    {
        public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            fighter.ResetHit();
            PlayRandomAnimation(impactAnimations);
        }

        public override void Tick(float deltaTime)
        {
            mover.Move(Vector3.zero);

            if(GetNormalizedTime("Impact") < 1) return;

            ReturnToLocomotion();
        }

        public override void Exit() { }
    }
}