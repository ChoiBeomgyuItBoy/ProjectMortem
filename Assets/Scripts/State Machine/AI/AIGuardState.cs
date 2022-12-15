using UnityEngine;

namespace Mortem.StateMachine.AI
{
    public class AIGuardState : AIBaseState
    {
        public AIGuardState(AIStateMachine stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
           PlayAnimationSmoothly("Locomotion");
        }

        public override void Tick(float deltaTime)
        {
            if(IsInChaseRange())
            {
                stateMachine.SwitchState(new AIChaseState(stateMachine));
                return;
            }
            else 
            {
                GuardBehaviour(deltaTime);
                return;
            }
        }

        public override void Exit()
        {
            ResetNavMeshAgent();
        }

        private void GuardBehaviour(float deltaTime)
        {
            if(AtGuardPosition())
            {
                stateMachine.SwitchState(new AIIdleState(stateMachine));
            }

            StartMoveAction(stateMachine.GuardPosition, stateMachine.WalkSpeed, 0.5f, deltaTime);
        }
    }
}