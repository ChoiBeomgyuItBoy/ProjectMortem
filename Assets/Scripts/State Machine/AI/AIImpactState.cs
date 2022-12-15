using Mortem.Combat;
using UnityEngine;

namespace Mortem.StateMachine.AI
{
    public class AIImpactState : AIBaseState
    {
        public AIImpactState(AIStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.GetComponent<Fighter>().ResetHit();
            PlayRandomAnimation(impactAnimations);
        }

        public override void Tick(float deltaTime)
        {
            mover.Move(Vector3.zero);
            LookAtPlayer();

            if(GetNormalizedTime("Impact") < 1) return;

            stateMachine.SwitchState(new AIIdleState(stateMachine));
        }

        public override void Exit() { } 
    }
}
