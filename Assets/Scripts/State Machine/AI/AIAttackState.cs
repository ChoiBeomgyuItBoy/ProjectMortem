using Mortem.Combat;
using Mortem.Core;
using UnityEngine;

namespace Mortem.StateMachine.AI
{
    public class AIAttackState : AIBaseState
    {
        private Attack attack;
  
        public AIAttackState(AIStateMachine stateMachine) : base(stateMachine) 
        {
            attack = stateMachine.GetComponent<Fighter>().Weapon.Combo[0];
        }

        public override void Enter()
        {
            PlayAnimationSmoothly(attack.AttackAnimation.name);
        }

        public override void Tick(float deltaTime)
        {
            mover.Move(Vector3.zero);
            LookAtPlayer();

            if(GetNormalizedTime("Attack") < 1) return;

            stateMachine.SwitchState(new AIChaseState(stateMachine));
        }

        public override void Exit() { }
    }
}
