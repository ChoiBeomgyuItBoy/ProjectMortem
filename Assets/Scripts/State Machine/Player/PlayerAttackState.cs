using UnityEngine;
using Mortem.Combat;

namespace Mortem.StateMachine.Player
{
    public class PlayerAttackState : PlayerBaseState
    {
        private Attack attack;

        private bool alreadyAppliedForce = false;

        private const float applyForceTime = 0.2f;

        public PlayerAttackState(PlayerStateMachine stateMachine, int comboIndex) : base(stateMachine)
        {
            attack = weapon.Combo[comboIndex];
        }

        public override void Enter()
        {
            PlayAnimationSmoothly(attack.AttackAnimation.name);
        }

        public override void Tick(float deltaTime)
        {
            mover.Move(Vector3.zero);
            targeter.FaceCurrentTarget(stateMachine.transform);

            TryApplyForce();

            if(input.IsAttacking)
            {
                TryComboAttack();
            }
            
            if(GetNormalizedTime("Attack") > 1f)
            {
                ReturnToLocomotion();
            }
        }

        public override void Exit() 
        { 
            forceReceiver.ResetImpact();
        }

        private void TryComboAttack()
        {
            if(attack.ComboIndex == -1) return;
            if(GetNormalizedTime("Attack") < attack.TimeBetweenAttacks) return;

            stateMachine.SwitchState(new PlayerAttackState(stateMachine, attack.ComboIndex));
        }

        private void TryApplyForce()
        {
            if(alreadyAppliedForce) return;
            if(GetNormalizedTime("Attack") < applyForceTime) return;
                
            forceReceiver.AddForce(stateMachine.transform.forward * attack.AttackForce);

            alreadyAppliedForce = true;
        }
    }
}
