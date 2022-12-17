using Mortem.Combat;
using UnityEngine;

namespace Mortem.StateMachine.AI
{
    public class AIImpactState : AIBaseState
    {
        public AIImpactState(AIStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Fighter.ResetHit();
            PlayRandomAnimation(impactAnimations);
        }

        public override void Tick(float deltaTime)
        {
            stateMachine.Mover.Move(Vector3.zero);
            LookAtPlayer();

            if(GetNormalizedTime("Impact") < 1) return;

            stateMachine.SwitchState(new AIIdleState(stateMachine));
        }

        public override void Exit() { } 
    }
}
