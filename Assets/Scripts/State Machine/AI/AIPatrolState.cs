using UnityEngine;

namespace Mortem.StateMachine.AI
{
    public class AIPatrolState : AIBaseState
    {
        private int currentWaypointIndex = 0;

        public AIPatrolState(AIStateMachine stateMachine) : base(stateMachine) { }

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
                PatrolBehaviour(deltaTime);
                return;
            }
        }

        public override void Exit()
        {
            ResetNavMeshAgent();
        }

        private void PatrolBehaviour(float deltaTime)
        {
            if(AtWaypoint())
            {
                CycleWaypoint();
            }

            StartMoveAction(GetCurrentWaypoint(), stateMachine.WalkSpeed, 0.5f, deltaTime);
        }

        private bool AtWaypoint()
        {
            return IsInRange(stateMachine.transform.position, GetCurrentWaypoint(), stateMachine.WaypointTolerance);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = stateMachine.PatrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return stateMachine.PatrolPath.GetWaypoint(currentWaypointIndex);
        }
    }
}