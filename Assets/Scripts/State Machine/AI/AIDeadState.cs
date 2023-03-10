using Mortem.Combat;
using UnityEngine;

namespace Mortem.StateMachine.AI
{
    public class AIDeadState : AIBaseState
    {
        public AIDeadState(AIStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            GameObject.Destroy(stateMachine.GetComponent<CombatTarget>());

            PlayAnimationSmoothly("Character_Die");

            stateMachine.GetComponent<CharacterController>().enabled = false;
            stateMachine.Agent.enabled = false;
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}
