using Mortem.Combat;
using Mortem.Control;
using Mortem.Movement;
using UnityEngine;

namespace Mortem.StateMachine.Player
{
    public class PlayerTargetState : PlayerBaseState
    {
        private PlayerInput input;
        private Targeter targeter;
        private Mover mover;


        public PlayerTargetState(PlayerStateMachine stateMachine) : base(stateMachine) 
        { 
            input = stateMachine.Input;
            targeter = stateMachine.Targeter;
            mover = stateMachine.Mover;
        }   

        public override void Enter()
        {
            input.TargetEvent += CancelTargetState;

            stateMachine.Fighter.ResetHit();

            PlayAnimationSmoothly("Targeting");
        }

        public override void Tick(float deltaTime)
        {
            TargetingBehaviour(deltaTime);

            if(input.IsAttacking)
            {
                stateMachine.SwitchState(new PlayerAttackState(stateMachine, 0));
                return;
            }

            if(targeter.CurrentTarget == null) 
            {
                stateMachine.SwitchState(new PlayerLocomotionState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            input.TargetEvent -= CancelTargetState;
        }

        private void TargetingBehaviour(float deltaTime)
        {
            targeter.FaceCurrentTarget(stateMachine.transform);
            mover.Move(CalculateMovement() * stateMachine.TargetingSpeed);

            SetAnimationBlendTree("TargetingRight", input.MovementValue.x, deltaTime);
            SetAnimationBlendTree("TargetingForward", input.MovementValue.y, deltaTime);
        }

        private void CancelTargetState()
        {
            targeter.Cancel();
            stateMachine.SwitchState(new PlayerLocomotionState(stateMachine));
        }

        private Vector3 CalculateMovement()
        {
            Vector3 direction = new Vector3();
            Vector3 right = stateMachine.transform.right;
            Vector3 forward = stateMachine.transform.forward;

            direction += (right * input.MovementValue.x);
            direction += (forward * input.MovementValue.y);

            return direction;
        }
    }
}
