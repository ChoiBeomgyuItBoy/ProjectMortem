using UnityEngine;
using Mortem.Combat;
using Mortem.Control;
using Mortem.Movement;
using Mortem.Core;

namespace Mortem.StateMachine.Player
{
    public abstract class PlayerBaseState : State
    {   
        protected PlayerStateMachine stateMachine;

        protected PlayerInput input;
        protected Mover mover;
        protected ForceReceiver forceReceiver;
        protected Fighter fighter;
        protected Weapon weapon;
        protected Targeter targeter;

        protected Vector3 currentMomentum;

        protected PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;

            input = stateMachine.GetComponent<PlayerInput>();
            mover = stateMachine.GetComponent<Mover>();
            forceReceiver = stateMachine.GetComponent<ForceReceiver>();
            fighter = stateMachine.GetComponent<Fighter>();
            weapon = stateMachine.GetComponent<Fighter>().Weapon;
            targeter = stateMachine.GetComponentInChildren<Targeter>();

            SetAnimator(stateMachine.GetComponent<Animator>());
        }

        protected void ReturnToLocomotion()
        {
            if(stateMachine.GetComponentInChildren<Targeter>().CurrentTarget == null)
            {
                stateMachine.SwitchState(new PlayerLocomotionState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerTargetState(stateMachine));
            }
        }

        protected void JumpBehaviour(string jumpAnimation, float force)
        {
            currentMomentum = forceReceiver.GetMomentum();
            forceReceiver.Jump(force);
            PlayAnimationSmoothly(jumpAnimation);
        }
    }
}