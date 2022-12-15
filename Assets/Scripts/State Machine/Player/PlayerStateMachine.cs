using UnityEngine;
using Mortem.Control;
using Mortem.Combat;
using System;

namespace Mortem.StateMachine.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public float JumpForce { get; private set; } = 4f;
        [field: SerializeField] public float DoubleJumpForce { get; private set; } = 3f;
        [field: SerializeField] public float FreeMovementSpeed { get; private set; } = 4f;
        [field: SerializeField] public float RunningSpeed { get; private set; } = 5f;
        [field: SerializeField] public float TargetingSpeed { get; private set; } = 4f;

        private void OnEnable()
        {
            GetComponent<Health>().DamageEvent += HandleDamage;
            GetComponent<Health>().DeadEvent += HandleDead;
        }

        private void OnDisable()
        {
            GetComponent<Health>().DamageEvent -= HandleDamage;
            GetComponent<Health>().DeadEvent -= HandleDead;
        }

        private void Start()
        {
            SwitchState(new PlayerLocomotionState(this));

            GetComponent<PlayerInput>().ToggleCursor(false);
        }

        private void HandleDamage()
        {
            SwitchState(new PlayerImpactState(this));
        }

        private void HandleDead()
        {
            SwitchState(new PlayerDeadState(this));
        }
    }
}
