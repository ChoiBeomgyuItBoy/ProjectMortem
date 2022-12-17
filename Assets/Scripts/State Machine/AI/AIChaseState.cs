using Mortem.Combat;
using UnityEngine;

namespace Mortem.StateMachine.AI
{
    public class AIChaseState : AIBaseState
    {
        public AIChaseState(AIStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Fighter.ResetHit();
            PlayAnimationSmoothly("Locomotion");
        }

        public override void Tick(float deltaTime)
        {
            Vector3 playerPosition = stateMachine.PlayerReference.transform.position;

            StartMoveAction(playerPosition, stateMachine.ChaseSpeed, 1f, deltaTime);

            if(!IsInChaseRange())
            {
                stateMachine.SwitchState(new AIIdleState(stateMachine));
            }
            else if(IsInAttackRange())
            {
                stateMachine.SwitchState(new AIAttackState(stateMachine));
            }
        }

        public override void Exit()
        {
            stateMachine.TimeSinceLastSawPlayer = 0f;
            ResetNavMeshAgent();
        }

        private bool IsInAttackRange()
        {
            if(stateMachine.PlayerReference.IsDead) return false;

            return IsInRange
            (
                stateMachine.transform.position, 
                stateMachine.PlayerReference.transform.position, 
                stateMachine.AttackRange
            );
        }
    }
}
