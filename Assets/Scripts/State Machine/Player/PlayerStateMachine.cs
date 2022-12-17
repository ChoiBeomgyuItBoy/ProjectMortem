using UnityEngine;
using Mortem.Control;
using Mortem.Combat;
using Mortem.Movement;
using Mortem.Core;

namespace Mortem.StateMachine.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public float JumpForce { get; private set; } = 4f;
        [field: SerializeField] public float DoubleJumpForce { get; private set; } = 3f;
        [field: SerializeField] public float FreeMovementSpeed { get; private set; } = 4f;
        [field: SerializeField] public float RunSpeed { get; private set; } = 5f;
        [field: SerializeField] public float TargetingSpeed { get; private set; } = 4f;

        public PlayerInput Input { get; private set; }
        public Mover Mover { get; private set; }
        public ForceReceiver ForceReceiver { get; private set; }
        public Targeter Targeter { get; private set; }
        public Health Health { get; private set; }
        public Fighter Fighter { get; private set; }
        public Weapon Weapon { get; private set; }

        public Animator Animator { get; private set; }
        public Transform MainCameraTransform { get; private set; }

        private void Awake()
        {
            Input = GetComponent<PlayerInput>();
            Mover = GetComponent<Mover>();
            
            ForceReceiver = GetComponent<ForceReceiver>();
            Targeter = GetComponentInChildren<Targeter>();

            Health = GetComponent<Health>();
            Fighter = GetComponent<Fighter>();
            Weapon = GetComponent<Fighter>().Weapon;

            Animator = GetComponent<Animator>();
            MainCameraTransform = Camera.main.transform;
        }

        private void OnEnable()
        {
            Health.DamageEvent += HandleDamage;
            Health.DeadEvent += HandleDead;
        }

        private void OnDisable()
        {
            Health.DamageEvent -= HandleDamage;
            Health.DeadEvent -= HandleDead;
        }

        private void Start()
        {
            SwitchState(new PlayerLocomotionState(this));

            Input.ToggleCursor(false);

            if(Weapon) Fighter.UnequipWeapon();
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
