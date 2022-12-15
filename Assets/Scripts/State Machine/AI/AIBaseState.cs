using Mortem.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace Mortem.StateMachine.AI
{
    public abstract class AIBaseState : State
    {
        protected AIStateMachine stateMachine;

        protected NavMeshAgent agent;
        protected Mover mover;

        public AIBaseState(AIStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;

            agent = stateMachine.GetComponent<NavMeshAgent>();
            mover = stateMachine.GetComponent<Mover>();

            SetAnimator(stateMachine.GetComponent<Animator>());
        }

        protected void ResetNavMeshAgent()
        {
            if(!agent.isOnNavMesh) return;

            stateMachine.GetComponent<NavMeshAgent>().ResetPath();
            stateMachine.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
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
            Vector3 desiredMovement = agent.desiredVelocity.normalized * speed;

            mover.LookAt(desiredMovement);

            SetAnimationBlendTree("MovementSpeed", blendTreeValue, deltaTime);

            if(!agent.isOnNavMesh) return;

            agent.destination = destination;
            mover.Move(desiredMovement);
        }

        protected void LookAtPlayer()
        {
            Vector3 lookPosition = stateMachine.PlayerReference.transform.position - stateMachine.transform.position;
            lookPosition.y = 0f;

            mover.LookAt(lookPosition);
        }

        protected bool AtGuardPosition()
        {
            return IsInRange(stateMachine.transform.position, stateMachine.GuardPosition, stateMachine.WaypointTolerance);
        }
    }
}
