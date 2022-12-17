using UnityEngine;

namespace Mortem.StateMachine.Player
{
    public abstract class PlayerBaseState : State
    {   
        protected PlayerStateMachine stateMachine;

        protected PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;

            SetAnimator(stateMachine.Animator);
        }

        protected void ReturnToLocomotion()
        {
            if(stateMachine.Targeter.CurrentTarget == null)
            {
                stateMachine.SwitchState(new PlayerLocomotionState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerTargetState(stateMachine));
            }
        }
    }
}