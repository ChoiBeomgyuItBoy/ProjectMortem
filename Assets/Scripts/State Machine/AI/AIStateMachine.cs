using UnityEngine.AI;
using UnityEngine;
using Mortem.Control;
using Mortem.Combat;

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

        public Vector3 GuardPosition { get; private set; }

        public Health PlayerReference { get; private set; }

        public float TimeSinceLastSawPlayer = Mathf.Infinity;

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
            SwitchState(new AIIdleState(this));

            PlayerReference = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

            ConfigNavMeshAgent();
            EquipWeapon();

            GuardPosition = transform.position;
        }

        private void EquipWeapon()
        {
            if(!TryGetComponent<Fighter>(out Fighter fighter)) return;
            if(!fighter.Weapon) Debug.LogError("Fighter Component Must Have a Weapon");
            fighter.EquipWeapon();
        }

        private void ConfigNavMeshAgent()
        {
            GetComponent<NavMeshAgent>().updatePosition = false;
            GetComponent<NavMeshAgent>().updateRotation = false;
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