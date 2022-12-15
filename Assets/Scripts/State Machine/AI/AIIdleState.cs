using Mortem.Combat;
using UnityEngine;

namespace Mortem.StateMachine.AI
{
    public class AIIdleState : AIBaseState
    {
        public AIIdleState(AIStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            PlayAnimationSmoothly("Locomotion");
        }

        public override void Tick(float deltaTime)
        {
            SetAnimationBlendTree("MovementSpeed", 0f, deltaTime);

            if(!stateMachine.GetComponent<Fighter>()) return;

            if(IsInChaseRange())
            {
                stateMachine.SwitchState(new AIChaseState(stateMachine));
                return;
            }
            else if(CanPatrol() && stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new AIPatrolState(stateMachine));
            }
            else if(CanPatrol() && !AtGuardPosition())
            {
                stateMachine.SwitchState(new AIGuardState(stateMachine));
            }

            stateMachine.TimeSinceLastSawPlayer += deltaTime;
        }

        public override void Exit() { }

        private bool CanPatrol()
        {
            return stateMachine.TimeSinceLastSawPlayer >= stateMachine.SuspicionTime;
        }
    }
}
