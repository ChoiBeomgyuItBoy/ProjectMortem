using UnityEngine;
using Mortem.Combat;

namespace Mortem.StateMachine.Player
{
    public class PlayerAttackState : PlayerBaseState
    {
        private Fighter fighter;
        private Attack attack;

        private bool alreadyAppliedForce = false;

        private const float applyForceTime = 0.2f;

        public PlayerAttackState(PlayerStateMachine stateMachine, int comboIndex) : base(stateMachine)
        {
            fighter = stateMachine.Fighter;
            attack = fighter.Weapon.Combo[comboIndex];
        }

        public override void Enter()
        {
            fighter.EquipWeapon();
            PlayAnimationSmoothly(attack.AttackAnimation.name);
        }

        public override void Tick(float deltaTime)
        {
            AttackBehaviour();

            if(stateMachine.Input.IsAttacking)
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
            fighter.UnequipWeapon();
        }

        private void AttackBehaviour()
        {
            stateMachine.Mover.Move(Vector3.zero);
            stateMachine.Targeter.FaceCurrentTarget(stateMachine.transform);

            TryApplyForce();
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
                
            stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.AttackForce);

            alreadyAppliedForce = true;
        }
    }
}
