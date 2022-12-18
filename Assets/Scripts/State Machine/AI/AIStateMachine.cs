using UnityEngine.AI;
using UnityEngine;
using Mortem.Control;
using Mortem.Combat;
using Mortem.Movement;

namespace Mortem.StateMachine.AI
{
    public class AIStateMachine : StateMachine
    {
        [field: SerializeField] public PatrolPath PatrolPath { get; private set; }
        [field: SerializeField] public float ChaseRange { get; private set; } = 5f;
        [field: SerializeField] public float AttackRange { get; private set; } = 2f;
        [field: SerializeField] public float WalkSpeed { get; private set; } = 3f;
        [field: SerializeField] public float ChaseSpeed { get; private set; } = 5f;
        [field: SerializeField] public float SuspicionTime { get; private set; } = 2f;
        [field: SerializeField] public float WaypointTolerance { get; private set; } = 1f;

        public NavMeshAgent Agent { get; private set; }
        public Mover Mover { get; private set; }
        public Fighter Fighter { get; private set; }
        public Weapon Weapon { get; private set; }
        public Health Health { get; private set; }

        public Vector3 GuardPosition { get; private set; }

        public Health PlayerReference { get; private set; }

        public float TimeSinceLastSawPlayer = Mathf.Infinity;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Mover = GetComponent<Mover>();
            Fighter = GetComponent<Fighter>();
            Weapon = GetComponent<Fighter>().Weapon;
            Health = GetComponent<Health>();

            PlayerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
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
            SwitchState(new AIIdleState(this));

            ConfigNavMeshAgent();
            EquipWeapon();

            GuardPosition = transform.position;
        }

        private void EquipWeapon()
        {
            if(!Weapon) 
            {
                Debug.LogError("Fighter Component Must Have a Weapon");
                return;
            }

            Fighter.EquipWeapon();
        }

        private void ConfigNavMeshAgent()
        {
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }

        private void HandleDamage()
        {
            SwitchState(new AIImpactState(this));
        }

        private void HandleDead()
        {
            SwitchState(new AIDeadState(this));
        }

        // Called in Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ChaseRange);
        }
    }
}