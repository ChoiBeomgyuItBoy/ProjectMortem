using Mortem.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace Mortem.StateMachine.AI
{
    public abstract class AIBaseState : State
    {
        protected AIStateMachine stateMachine;

        public AIBaseState(AIStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;

            SetAnimator(stateMachine.GetComponent<Animator>());
        }

        protected void ResetNavMeshAgent()
        {
            if(!stateMachine.Agent.isOnNavMesh) return;

            stateMachine.Agent.ResetPath();
            stateMachine.Agent.velocity = Vector3.zero;
        }

        protected bool IsInChaseRange()
        {
            if(stateMachine.PlayerReference.IsDead) return false;

            return IsInRange
            (
                stateMachine.transform.position, 
                stateMachine.PlayerReference.transform.position,
                stateMachine.ChaseRange
            );
        }

        protected bool IsInRange(Vector3 from, Vector3 to, float range)
        {
            float targetDistanceSqr  = (from - to).sqrMagnitude;
            float rangeSqr = range * range;

            return targetDistanceSqr < rangeSqr;
        }

        protected void StartMoveAction(Vector3 destination, float speed, float blendTreeValue, float deltaTime)
        {
            Vector3 desiredMovement = stateMachine.Agent.desiredVelocity.normalized * speed;

            stateMachine.Mover.LookAt(desiredMovement);

            SetAnimationBlendTree("MovementSpeed", blendTreeValue, deltaTime);

            if(!stateMachine.Agent.isOnNavMesh) return;

            stateMachine.Agent.destination = destination;
            stateMachine.Mover.NavMeshAgentMove(desiredMovement);
        }

        protected void LookAtPlayer()
        {
            Vector3 lookPosition = stateMachine.PlayerReference.transform.position - stateMachine.transform.position;
            lookPosition.y = 0f;

            stateMachine.Mover.LookAt(lookPosition);
        }

        protected bool AtGuardPosition()
        {
            return IsInRange(stateMachine.transform.position, stateMachine.GuardPosition, stateMachine.WaypointTolerance);
        }
    }
}
